using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketIcon : EnemyIcon<Rocket>
{
    protected override string GetEnemyActionDescriptionLine()
    {
        return $"Deals {Enemy.Damage} damage on colision";
    }
}
