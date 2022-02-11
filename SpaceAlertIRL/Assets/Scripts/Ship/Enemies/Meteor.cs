using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Meteor : Enemy
{
    protected override int StratingHPConst => 5;
    protected override int StartingEnergyShieldsConst => 0;

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

        int _newHP = HP.Value - damage;
        if (_newHP > 0)
        { HP.Value = _newHP; }
        else
        {
            HP.Value = 0;
            Die();
        }
    }
}
