using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeteorIcon : EnemyIcon<Meteor>
{
    protected override string GetEnemyActionDescriptionLine()
    {
        return $"Attack ({Enemy.HP}) in {(Enemy.Distance / Enemy.Speed).ToString("0.00")}";
    }
}
