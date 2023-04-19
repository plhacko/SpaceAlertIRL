using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeteorIcon : EnemyIcon<Meteor>
{
    protected override string GetEnemyActionDescriptionLine()
    {
        return $"Impact ({Enemy.HP}) in {(Enemy.Distance / Enemy.Speed).ToString("0.0")}";
    }
}
