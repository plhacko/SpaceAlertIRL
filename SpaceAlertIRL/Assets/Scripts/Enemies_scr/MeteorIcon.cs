using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeteorIcon : EnemyIcon<Meteor>
{
    protected override string GetEnemyActionDescriptionLine()
    {
        return $"Deals {Enemy.HP} damage on impact.";
    }
}
