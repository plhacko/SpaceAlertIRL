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
            string line1 = $"Meteor, HP : {Enemy.HP}/{Enemy.MaxHP}, ES : {Enemy.EnergyShields}/{Enemy.MaxEnergyShields}";
            string line2 = $"Damage: {Enemy.HP}";
            string line3 = $"Impact in : {Enemy.Impact}";
            GetComponentInChildren<TextMeshProUGUI>().text = line1 + '\n' + line2 + '\n' + line3;
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
