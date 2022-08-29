using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RailgunActionPanel : AmenityActionPanel<Railgun>
{
    public override void Initialise(Railgun railgun)
    {
        base.Initialise(railgun);
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(Amenity.Zone);
    }

    protected override void UpdateUI()
    {
        var _damage = Amenity.GetRangeValue();
        var _range = Amenity.GetRangeValue();
        var _chargingTime = Amenity.GetChargingTimeValue();
        var _timeToChargeConst = Amenity.GetTimeToChargeConst();

        var _chargePercentage = 100.0f *_chargingTime / _timeToChargeConst;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>().text = $"Damage : {_damage}";
        transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>().text = $"Range : {_range}";
        transform.Find("Charge").GetComponentInChildren<TextMeshProUGUI>().text = $"Charge : {_chargePercentage.ToString("0.##\\%")}";
    }

    public void RequestShootingAtClosesEnemy()
    {
        Amenity.RequestShootingAtClosesEnemy();
    }

    public void RequestCharging()
    {
        Amenity.RequestCharging();
    }
}