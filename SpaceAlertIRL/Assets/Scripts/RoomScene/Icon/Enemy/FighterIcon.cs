using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FighterIcon : EnemyIcon<Fighter>
{
    protected override void UpdateUI()
    {
        if (Enemy != null)
        {
            string line1 = $"Fighter, HP : {Enemy.HP}/{Enemy.MaxHP}, ES : {Enemy.EnergyShield}/{Enemy.MaxEnergyShield}";
            string line2 = $"{Enemy.NextActionDescription} in {Enemy.NextActionTime.ToString("0.00")}"; // TODO: redo this
            string line3 = $"Distance : {Enemy.Distance.ToString("0.00")}";
            GetComponentInChildren<TextMeshProUGUI>().text = line1 + '\n' + line2 + '\n' + line3;
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
