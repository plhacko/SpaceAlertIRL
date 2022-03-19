#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Enemy : NetworkBehaviour
{
    [SerializeField]
    protected GameObject IconPrefab;

    abstract protected int StratingHPConst { get; }
    abstract protected int MaxEnergyShieldConst { get; }
    abstract protected float StartingSpeedConst { get; }
    abstract protected float StartingDistanceConst { get; }

    abstract protected float EnergyShieldRegenerationTimeConst { get; }

    protected NetworkVariable<int> _HP;
    protected NetworkVariable<int> _EnergyShield;
    protected NetworkVariable<float> _EnergyShieldRegenerationTime;
    protected NetworkVariable<float> _Speed;
    protected NetworkVariable<float> _Distance;

    // geters for UI
    public int HP { get => _HP.Value; }
    public int MaxHP { get => StratingHPConst; }
    public int EnergyShield { get => _EnergyShield.Value; }
    public int MaxEnergyShield { get => MaxEnergyShieldConst; }
    public float Distance { get => _Distance.Value; }
    public float Speed { get => _Speed.Value; }

    protected Zone Zone;

    public UpdateUIActions UIActions = new UpdateUIActions();

    public abstract void SpawnIconAsChild(GameObject parent);

    protected virtual void Start()
    {
        _HP = new NetworkVariable<int>(StratingHPConst);
        _EnergyShield = new NetworkVariable<int>(MaxEnergyShieldConst);
        _EnergyShieldRegenerationTime = new NetworkVariable<float>(0.0f);
        _Distance = new NetworkVariable<float>(StartingDistanceConst);
        _Speed = new NetworkVariable<float>(StartingSpeedConst);

        UIActions.AddOnValueChangeDependency(_HP);
        UIActions.AddOnValueChangeDependency(_EnergyShield);
        UIActions.AddOnValueChangeDependency(_EnergyShieldRegenerationTime);
        UIActions.AddOnValueChangeDependency(_Distance);
        UIActions.AddOnValueChangeDependency(_Speed);

        Zone = GetComponentInParent<Zone>();
        Zone.AddEnemy(this);
        Zone.UIActions.UpdateUI();
    }
#if SERVER
    void FixedUpdate()
    {
        // Update should tun only on server
        if (!NetworkManager.Singleton.IsServer) { return; }

        DistanceChange();
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

#endif

    protected virtual void Impact()
    {
        Zone.RemoveEnemy(this);
        GetComponent<NetworkObject>().Despawn(true);
    }

    virtual public void TakeDamage(int damage, Weapon w) // the weapon is needed if there is a special exception
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
        _HP.Value = 0;
        Zone.RemoveEnemy(this);
        GetComponent<NetworkObject>().Despawn(true);
    }
}

abstract public class Enemy<T> : Enemy where T : Enemy<T>
{
    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<EnemyIcon<T>>().Initialise((T)this);
    }
}
