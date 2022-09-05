using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserActionPanel : AmenityActionPanel<Laser>
{
    public override void Initialise(Laser laser)
    {
        base.Initialise(laser);
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(Amenity.Zone);
    }

    protected override void UpdateUI()
    {
        var _damage = Amenity.GetWeaponDamage();
        var _range = Amenity.GetWeaponRange();
        var _heat = Amenity.GetWeaponHeat();
        bool _tooHot = Amenity.IsTooHotToShoot();
        string _statusTest = _tooHot ? "high heat" : "ok";

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = $"Status : {_statusTest}"; 
        transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>().text = $"Damage : {_damage}";
        transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>().text = $"Range : {_range}";
        transform.Find("Heat").GetComponentInChildren<TextMeshProUGUI>().text = $"Heat : {_heat.ToString("0.00\\%")}";
    }

    public void RequestShootingAtClosesEnemy()
    {
        Amenity.RequestShootingAtClosesEnemy();
    }
}
