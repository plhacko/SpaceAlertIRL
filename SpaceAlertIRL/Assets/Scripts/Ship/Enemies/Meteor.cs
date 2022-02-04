using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void TakeDamage(Weapon w)
    {
        
    }
}
