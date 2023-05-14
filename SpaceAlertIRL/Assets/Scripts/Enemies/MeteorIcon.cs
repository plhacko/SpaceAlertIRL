using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeteorIcon : EnemyIcon<Meteor>
{
    protected override string GetEnemyActionDescriptionLine()
    {
        return $"Deals _{Enemy.HP}_ damage on impact.";
    }
}
