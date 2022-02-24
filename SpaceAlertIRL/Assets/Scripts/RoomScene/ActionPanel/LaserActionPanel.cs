using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserActionPanel : ActionPanel<Laser>
{
    
    public override void Initialise(Laser laser)
    {
        base.Initialise(laser);
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(Amenity.Zone);
    }

    protected override void UpdateUI()
    {
        var _damage = Amenity.Damage.Value;
        var _range = Amenity.Range.Value;
        var _heat = Amenity.Heat.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>().text = $"Damage : {_damage}";
        transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>().text = $"Range : {_range}";
        transform.Find("Heat").GetComponentInChildren<TextMeshProUGUI>().text = $"Heat : {_heat}";
    }


    public void RequestShootingAtClosesEnemy()
    {
        Amenity.RequestShootingAtClosesEnemyServerRpc();
    }
}
