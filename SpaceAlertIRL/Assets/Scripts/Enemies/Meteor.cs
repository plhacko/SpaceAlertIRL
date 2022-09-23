#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class Meteor : Enemy<Meteor>
{
    protected override int StratingHPConst => 5;
    protected override int MaxEnergyShieldConst => 0;
    protected override float EnergyShieldRegenerationTimeConst => 10.0f;
    protected override float StartingSpeedConst => 0.5f;
    protected override float StartingDistanceConst => 40;

    protected override void Impact()
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server"); }

        Zone.TakeDmage(this.HP, this);
        base.Impact();
    }

    public override void TakeDamage(int damage)
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

    protected override EnemyAction DecideNextAction() => new Wait(float.MaxValue);
}
