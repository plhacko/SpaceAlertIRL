#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class Meteor : Enemy
{
    protected override int StratingHPConst => 5;
    protected override int StartingEnergyShieldsConst => 0;
    protected virtual int StartingImpactConst => 15;

    [SerializeField]
    protected NetworkVariable<int> _Impact;

    public int Impact { get => _Impact.Value; } // mostly method for UI

#if (SERVER)
    float Impact_serverVariable; // the actual time is stored on server, so the NetworkVariable doesn't need to synchronize so much
    void Update()
    {
        // Update should tun only on server
        if (!NetworkManager.Singleton.IsServer) { return; }

        Impact_serverVariable -= Time.deltaTime;
        if ((int)Impact_serverVariable != _Impact.Value)
        {
            _Impact.Value = (int)Impact_serverVariable;
            if (_Impact.Value == 0) { Die(); }
        }
    }
#endif

    protected override void Start()
    {
        base.Start();
        _Impact = new NetworkVariable<int>(StartingImpactConst);
#if (SERVER)
        Impact_serverVariable = (float)StartingImpactConst;
#endif
    }

    protected void DoDamage()
    {
        // does damage equal to it's HP and dies
        Zone.TakeDmage(this.HP);
        Die();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void SpawnIconAsChild(GameObject parent)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(int damage, Weapon w)
    {
        if (damage < 0) { Debug.Log("damage can't be negative"); return; }

        int _newHP = _HP.Value - damage;
        if (_newHP > 0)
        { _HP.Value = _newHP; }
        else
        {
            _HP.Value = 0;
            Die();
        }
    }
}
