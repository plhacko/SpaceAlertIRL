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

    abstract protected int StratingHPConst { get; }
    abstract protected int MaxEnergyShieldConst { get; }
    abstract protected float StartingSpeedConst { get; }
    virtual protected RangeEnum StartingDistanceConst { get => RangeEnum.Far; }
    abstract protected float EnergyShieldRegenerationTimeConst { get; }

    // geters for data
    public int HP { get => _HP.Value; }
    public int MaxHP { get => StratingHPConst; }
    public int EnergyShield { get => _EnergyShield.Value; }
    public int MaxEnergyShield { get => MaxEnergyShieldConst; }
    public float Distance { get => _Distance.Value; }
    public float Speed { get => _Speed.Value; set => _Speed.Value = value; }
    public float NextActionTime { get => _NextActionTime.Value; }
    public virtual bool IsTragetabeByRocket { get => true; }
    public string NextActionDescription { get => _NextActionDescription.Value.ToString(); }

    // setters for data
    public void SetDistance(float newDistance) { _Distance.Value = newDistance; }

    // NetworkVariables
    protected NetworkVariable<int> _HP = new NetworkVariable<int>();
    protected NetworkVariable<int> _EnergyShield = new NetworkVariable<int>();
    protected NetworkVariable<float> _EnergyShieldRegenerationTime = new NetworkVariable<float>();
    protected NetworkVariable<float> _Speed = new NetworkVariable<float>();
    protected NetworkVariable<float> _Distance = new NetworkVariable<float>();
    protected NetworkVariable<float> _NextActionTime = new NetworkVariable<float>();
    protected NetworkVariable<FixedString64Bytes> _NextActionDescription = new NetworkVariable<FixedString64Bytes>();

    protected Zone Zone;

    public UpdateUIActions UIActions = new UpdateUIActions();
    RectTransform DistanceMeter;
    GameObject DistanceMeterIcon;
    Line UILine;

    public abstract void SpawnIconAsChild(GameObject parent);


    public virtual void Start()
    {
        DistanceMeter = transform.parent.GetComponent<EnemySpawner>().GetDistanceMeter().GetComponent<RectTransform>();
        DistanceMeterIcon = transform.Find("DistanceMeterIcon").gameObject;
        UILine = GetComponentInChildren<Line>();

        UIActions.AddOnValueChangeDependency(_HP, _EnergyShield);
        UIActions.AddOnValueChangeDependency(_Distance, _Speed, _NextActionTime);
        UIActions.AddOnValueChangeDependency(_NextActionDescription);

        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();
    }

    public virtual void Initialise()
    {
        _HP.Value = StratingHPConst;
        _EnergyShield.Value = MaxEnergyShieldConst;
        _EnergyShieldRegenerationTime.Value = 0.0f;
        _Distance.Value = (float)StartingDistanceConst;
        _Speed.Value = StartingSpeedConst;
        _NextActionTime.Value = 0.0f;
        _NextActionDescription.Value = "";

        Zone = GetComponentInParent<Zone>();

        ServerUpdater.Add(this.gameObject);
    }
#if SERVER

    void IOnServerFixedUpdate.ServerFixedUpdate()
    {
        DistanceChange();
        EnergyShieldsRegeneration();
        ActionTimer();
    }

    protected virtual void DistanceChange()
    {
        float newDistance = _Distance.Value - Time.deltaTime * _Speed.Value;
        if (newDistance > 0)
        { _Distance.Value = newDistance; }
        else
        { _Distance.Value = 0; Impact(); }
    }
    protected virtual void EnergyShieldsRegeneration()
    {
        if (_EnergyShield.Value == MaxEnergyShieldConst)
        { _EnergyShieldRegenerationTime.Value = 0; }
        else
        {
            float newTime = _EnergyShieldRegenerationTime.Value + Time.deltaTime;
            if (newTime < EnergyShieldRegenerationTimeConst)
            {
                _EnergyShieldRegenerationTime.Value = newTime;
            }
            else
            {
                _EnergyShieldRegenerationTime.Value = 0.0f;
                _EnergyShield.Value = MaxEnergyShieldConst;
            }
        }
    }
    EnemyAction NextEnemyAction;
    protected abstract EnemyAction DecideNextAction();
    protected virtual void ActionTimer()
    {
        if (NextEnemyAction == null)
        {
            NextEnemyAction = DecideNextAction();
            _NextActionDescription.Value = NextEnemyAction.GetDescription() ?? "no action";
        }

        float newTime = _NextActionTime.Value - Time.deltaTime;
        if (newTime <= 0)
        {
            NextEnemyAction.ExecuteAction();
            NextEnemyAction = DecideNextAction();
            _NextActionDescription.Value = NextEnemyAction.GetDescription() ?? "no action";
            _NextActionTime.Value = NextEnemyAction.TimeSpan;
        }
        else
        { _NextActionTime.Value = newTime; }
    }

#endif

    protected virtual void Impact()
    {
        _HP.Value = 0;
        GetComponent<NetworkObject>().Despawn(true);
    }

    virtual public void TakeDamage(int damage) // the weapon is needed if there is a special exception
    {
        if (damage < 0) { Debug.Log("damage can't be negative"); return; }

        // damageToShields
        int damageToShields = System.Math.Min(_EnergyShield.Value, damage);
        damage -= damageToShields;
        _EnergyShield.Value = _EnergyShield.Value - damageToShields;

        // damaheTuHull
        int _newHP = _HP.Value - damage;
        if (_newHP > 0)
        { _HP.Value = _newHP; }
        else
        {
            _HP.Value = 0;
            Die();
        }

        // shield regeneratin will start all over
        _EnergyShieldRegenerationTime.Value = 0;
    }
    virtual public void Die(bool silent = false)
    {
        if (!silent)
        {
            string _zoneName = GetComponentInParent<Zone>().gameObject.name + "_r";
            AudioManager.Instance.RequestPlayingSentenceOnClient($"{_zoneName} enemyTerminated_r", removeDuplicates: false);
        }

        _HP.Value = 0;
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

        // destroy DistanceMeterIcon if this enemy dies
        if (HP == 0)
        {
            Destroy(DistanceMeterIcon);
        }
    }
}

abstract public class Enemy<T> : Enemy where T : Enemy<T>
{
    public override string GetName() => typeof(T).Name;

    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent: parent.transform);
        _go.GetComponent<EnemyIcon<T>>().Initialise((T)this);
    }
}

