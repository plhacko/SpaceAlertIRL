#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;
using UnityEngine.UI;

public abstract class Enemy : NetworkBehaviour, IComparable<Enemy>, IOnServerFixedUpdate
{
    [SerializeField]
    protected GameObject IconPrefab;

    abstract public string GetName();

    // constants
    abstract protected int StratingHPConst { get; }
    abstract protected int MaxEnergyShieldConst { get; }
    abstract protected float StartingSpeedConst { get; }
    virtual protected RangeEnum StartingDistanceConst { get => RangeEnum.Far; }
    virtual protected float EnergyShieldRegenerationTimeConst { get => 5.0f; }

    // getters and setters for data (is mostly a wrapper for network variables)
    public int HP { get => _HP.Value; set { _HP.Value = value; } }
    public int MaxHP { get => StratingHPConst; }
    public int EnergyShield { get => _EnergyShield.Value; set { _EnergyShield.Value = value; } }
    public int MaxEnergyShield { get => MaxEnergyShieldConst; }
    public float EnergyShieldRegenerationTime { get => _EnergyShieldRegenerationTime.Value; set { _EnergyShieldRegenerationTime.Value = value; } }
    public float Distance { get => _Distance.Value; set { _Distance.Value = value; } }
    public float Speed { get => _Speed.Value; set => _Speed.Value = value; }
    public virtual bool IsTragetabeByRocket { get => true; }
    public string NextActionDescription { get => _NextActionDescription.Value.ToString(); set { _NextActionDescription.Value = value; } }

    // NetworkVariables
    protected NetworkVariable<int> _HP = new NetworkVariable<int>();
    protected NetworkVariable<int> _EnergyShield = new NetworkVariable<int>();
    protected NetworkVariable<float> _EnergyShieldRegenerationTime = new NetworkVariable<float>();
    protected NetworkVariable<float> _Speed = new NetworkVariable<float>();
    protected NetworkVariable<float> _Distance = new NetworkVariable<float>();
    protected NetworkVariable<FixedString128Bytes> _NextActionDescription = new NetworkVariable<FixedString128Bytes>();

    protected Zone Zone;

    public UpdateUIActions UIActions = new UpdateUIActions();
    RectTransform DistanceMeter;
    GameObject DistanceMeterIcon;
    Line UILine;

    public abstract GameObject SpawnIconAsChild(GameObject parent);

    public virtual void Start()
    {
        DistanceMeter = transform.parent.GetComponent<EnemySpawner>().GetDistanceMeter().GetComponent<RectTransform>();
        DistanceMeterIcon = transform.Find("DistanceMeterIcon").gameObject;
        UILine = GetComponentInChildren<Line>();

        UIActions.AddOnValueChangeDependency(_HP, _EnergyShield);
        UIActions.AddOnValueChangeDependency(_Distance, _Speed);
        UIActions.AddOnValueChangeDependency(_NextActionDescription);

        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();
    }

    public virtual void Initialise()
    {
        HP = StratingHPConst;
        EnergyShield = MaxEnergyShieldConst;
        EnergyShieldRegenerationTime = 0.0f;
        Distance = (float)StartingDistanceConst;
        Speed = StartingSpeedConst;
        NextActionDescription = "";

        Zone = GetComponentInParent<Zone>();

        // choose first action
        NextEnemyAction = DecideNextAction();
        NextActionDescription = NextEnemyAction.GetDescription();

        ServerUpdater.Add(this.gameObject);
    }
#if SERVER

    void IOnServerFixedUpdate.ServerFixedUpdate()
    {
        DistanceChange(); // may trigger action
        EnergyShieldsRegeneration();
    }

    private bool wasActionAnnounced = false;
    protected virtual void DistanceChange(bool silent = false)
    {
        float newDistance = Distance - Time.deltaTime * Speed;

        // voice announcement, that an action will happen in 15sec
        if (!wasActionAnnounced && IsLessThan15secFromTheNextAction(newDistance))
        {
            if (!silent) { AudioManager.Instance.RequestPlayingSentenceOnClient($"{Zone.name + "_r"} enemyActionIn15seconds_r"); }
            wasActionAnnounced = true;
        }

        // action triggers if the ship passed Mid/Close/Zero ranges
        if (newDistance <= (int)RangeEnum.Mid && Distance > (int)RangeEnum.Mid
            || newDistance <= (int)RangeEnum.Close && Distance > (int)RangeEnum.Close
            || newDistance <= (int)RangeEnum.Zero && Distance > (int)RangeEnum.Zero)
        {
            if (!silent && !NextEnemyAction.IsSilent) { AudioManager.Instance.RequestPlayingSentenceOnClient($"{Zone.name + "_r"} enemyActionExecuted_r"); }
            NextEnemyAction?.ExecuteAction();
            NextEnemyAction = DecideNextAction();
            NextActionDescription = NextEnemyAction.GetDescription() ?? "no action";
            wasActionAnnounced = false;
        }

        // normal movement (sets distance)
        if (newDistance > 0)
        { Distance = newDistance; }
        else { Distance = 0; Impact(); }
    }
    protected virtual bool IsLessThan15secFromTheNextAction(float distance)
    {
        foreach (RangeEnum range in new RangeEnum[] { RangeEnum.Mid, RangeEnum.Close, RangeEnum.Zero })
        {
            float delta = (distance - (int)range) / Speed;
            if (delta < 15.0f && delta > 0.0f)
            { return true; }
        }
        return false;
    }

    protected virtual void EnergyShieldsRegeneration()
    {
        if (EnergyShield == MaxEnergyShieldConst)
        { EnergyShieldRegenerationTime = 0; }
        else
        {
            float newTime = EnergyShieldRegenerationTime + Time.deltaTime;
            if (newTime < EnergyShieldRegenerationTimeConst)
            {
                EnergyShieldRegenerationTime = newTime;
            }
            else
            {
                EnergyShieldRegenerationTime = 0.0f;
                EnergyShield = MaxEnergyShieldConst;
            }
        }
    }
    protected EnemyAction NextEnemyAction;
    protected abstract EnemyAction DecideNextAction();

#endif
    public int DepleteEnergyShields(int damage)
    {
        int damageToShields = System.Math.Min(EnergyShield, damage);
        EnergyShield -= damageToShields;
        return damage - damageToShields;
    }

    virtual public void TakeDamage(int damage, bool silent = false) // the weapon is needed if there is a special exception
    {
        if (damage < 0) { Debug.Log("damage can't be negative"); return; }

        // damage to shields
        damage = DepleteEnergyShields(damage);

        // damage to hull
        int newHP = HP - damage;
        if (newHP > 0)
        {
            HP = newHP;
            if (!silent) { AudioManager.Instance.RequestPlayingSentenceOnClient($"{Zone.name + "_r"} enemyDamaged_r"); }
        }
        else
        { Die(); }

        // shield regeneratin will start all over
        EnergyShieldRegenerationTime = 0;
    }
    protected virtual void Impact(bool silent = false)
    {
        if (!silent)
        {
            string zoneName = GetComponentInParent<Zone>().gameObject.name + "_r";
            AudioManager.Instance.RequestPlayingSentenceOnClient($"{zoneName} enemyLeft_r", removeDuplicates: false);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail);
        }
        HP = 0;
        GetComponent<NetworkObject>().Despawn(true);
    }

    public virtual bool IsDead => (HP <= 0);
    virtual public void Die(bool silent = false)
    {
        if (!silent)
        {
            string zoneName = GetComponentInParent<Zone>().gameObject.name + "_r";
            AudioManager.Instance.RequestPlayingSentenceOnClient($"{zoneName} enemyTerminated_r", removeDuplicates: false);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success);
        }

        HP = 0;
        GetComponent<NetworkObject>().Despawn();
    }
    public int CompareTo(Enemy e)
    {
        if (e == null) { return 1; }
        return Distance.CompareTo(e.Distance);
    }

    void UpdateUI()
    {
        // set layoutPriority
        var otherEnemies = transform.parent.GetComponentsInChildren<Enemy>();
        int i = otherEnemies.Length - 1;
        foreach (Enemy e in otherEnemies)
        { if (e.Distance > Distance) { i--; } }
        transform.SetSiblingIndex(i);

        // set DistanceMeterIcon
        try
        {
            if (DistanceMeterIcon.transform.parent != DistanceMeter.transform)
            {
                DistanceMeterIcon.transform.SetParent(DistanceMeter);
                DistanceMeterIcon.transform.localScale = Vector3.one;
                transform.localScale = Vector3.one;
            }
            float offset = DistanceMeter.sizeDelta.y / 2;
            var distance = Distance / (int)RangeEnum.Far;

            DistanceMeterIcon.transform.localPosition = new Vector3(0, offset - 2 * offset * distance, 0);

            // set DistanceMeterIcon color
            DistanceMeterIcon.GetComponent<Image>().color = ProjectColors.GetColorForDistance(Distance);

            // set line
            UILine.UpdateUI(DistanceMeterIcon.transform, transform);

        }
        catch (Exception)
        {
            Debug.Log($"No DistanceMeterIcon setup in icon of enemy \"{this.GetName()}\"");
            Debug.Log($"DistanceMeterIcon {DistanceMeterIcon}\n DistanceMeter {DistanceMeter}\nUILine {UILine}");
        }
    }

    protected virtual void OnDisable()
    {
        if (DistanceMeterIcon != null)
        { Destroy(DistanceMeterIcon); }
    }
}

abstract public class Enemy<T> : Enemy where T : Enemy<T>
{
    public override string GetName() => typeof(T).Name;

    public override GameObject SpawnIconAsChild(GameObject parent)
    {
        GameObject go = Instantiate(IconPrefab, parent: parent.transform);
        go.GetComponent<EnemyIcon<T>>().Initialise((T)this);
        return go;
    }
}

