using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeteorIcon : EnemyIcon<Meteor>
{
    protected override void UpdateUI()
    {
        if (Enemy != null)
        {
            string line1 = GetEnemyNemeHpEsLine();
            string line2 = $"Attack ({Enemy.HP}) in {(Enemy.Distance/Enemy.Speed).ToString("0.00")}";
            string line3 = GetEnemyDistanceLine();
            GetComponentInChildren<TextMeshProUGUI>().text = line1 + '\n' + line2 + '\n' + line3;
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
