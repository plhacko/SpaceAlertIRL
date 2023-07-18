#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class Meteor : Enemy<Meteor>
{
    protected override int StratingHPConst => 7;
    protected override int MaxEnergyShieldConst => 0;
    protected override float StartingSpeedConst => 3.5f;

    protected override void Impact(bool silent = false)
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server"); }

        Zone.TakeDmage(this.HP, this);
        base.Impact(silent);
    }

    protected override bool IsLessThan15secFromTheNextAction(float distance)
    {
        foreach (RangeEnum range in new RangeEnum[] { RangeEnum.Zero })
        {
            float delta = (distance - (int)range) / Speed;
            if (delta < 15.0f && delta > 0.0f)
            { return true; }
        }
        return false;
    }

    protected override EnemyAction DecideNextAction() => new Wait();
}
