#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;

public abstract class Enemy : NetworkBehaviour, IComparable<Enemy>, IOnServerFixedUpdate
{
    [SerializeField]
    protected GameObject IconPrefab;

    abstract public string GetName();

    abstract protected int StratingHPConst { get; }
    abstract protected int MaxEnergyShieldConst { get; }
    abstract protected float StartingSpeedConst { get; }
    abstract protected float StartingDistanceConst { get; }
    abstract protected float EnergyShieldRegenerationTimeConst { get; }

    // geters for data
    public int HP { get => _HP.Value; }
    public int MaxHP { get => StratingHPConst; }
    public int EnergyShield { get => _EnergyShield.Value; }
    public int MaxEnergyShield { get => MaxEnergyShieldConst; }
    public float Distance { get => _Distance.Value; }
    public float Speed { get => _Speed.Value; }
    public float NextActionTime { get => _NextActionTime.Value; }
    public virtual bool IsTragetabeByRocket() => true;
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
    protected NetworkVariable<FixedString32Bytes> _NextActionDescription = new NetworkVariable<FixedString32Bytes>();

    protected Zone Zone;

    public UpdateUIActions UIActions = new UpdateUIActions();
    public abstract void SpawnIconAsChild(GameObject parent);

    public bool IsInitialised { get; private set; } = false;

    public virtual void Start()
    {
        UIActions.AddOnValueChangeDependency(_HP, _EnergyShield);
        UIActions.AddOnValueChangeDependency(_Distance, _Speed, _NextActionTime);
        UIActions.AddOnValueChangeDependency(_NextActionDescription);
    }

    public virtual void Initialise(Zone z)
    {
        IsInitialised = true;

        _HP.Value = StratingHPConst;
        _EnergyShield.Value = MaxEnergyShieldConst;
        _EnergyShieldRegenerationTime.Value = 0.0f;
        _Distance.Value = StartingDistanceConst;
        _Speed.Value = StartingSpeedConst;
        _NextActionTime.Value = 0.0f;
        _NextActionDescription.Value = "";

        // Zone = GetComponentInParent<Zone>(); // TODO: rm
        Zone = z;
        // Zone.AddEnemy(this); // TODO: rm
        // Zone.UIActions.UpdateUI();
    }
#if SERVER

    void IOnServerFixedUpdate.ServerFixedUpdate()
    {
        // Update should tun only on server
        if (!NetworkManager.Singleton.IsServer || !IsInitialised) { return; } // TODO: rm

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
        // Zone.RemoveEnemy(this); TODO: rm
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
    }
    virtual public void Die()
    {
        string _zoneName = GetComponentInParent<Zone>().gameObject.name + "_r";
        GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient($"{_zoneName} enemyTerminated_r", removeDuplicates: false);

        _HP.Value = 0;
        // Zone.RemoveEnemy(this); TODO: rm
        GetComponent<NetworkObject>().Despawn();
    }
    public int CompareTo(Enemy e)
    {
        if (e == null) { return 1; }
        return Distance.CompareTo(e.Distance);
    }
}

abstract public class Enemy<T> : Enemy where T : Enemy<T>
{
    public override string GetName() => typeof(T).Name;

    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<EnemyIcon<T>>().Initialise((T)this);
    }
}

