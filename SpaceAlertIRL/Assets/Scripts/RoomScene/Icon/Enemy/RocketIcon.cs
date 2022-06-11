using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketIcon : EnemyIcon<Rocket>
{
    protected override void UpdateUI()
    {
        if (Enemy != null)
        {
            string line1 = GetEnemyNemeHpEsLine();
            string line2 = $"Attack({Enemy.Damage}) on colision";
            string line3 = GetEnemyDistanceLine();
            GetComponentInChildren<TextMeshProUGUI>().text = line1 + '\n' + line2 + '\n' + line3;
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
