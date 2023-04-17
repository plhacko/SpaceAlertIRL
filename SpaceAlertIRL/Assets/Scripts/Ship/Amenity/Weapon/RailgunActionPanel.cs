using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RailgunActionPanel : AmenityActionPanel<Railgun>
{
    TextMeshProUGUI Status_text, Damage_text, Range_text, Charge_text;
    Image Range_image;

    private void Awake()
    {
        Status_text = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Damage_text = transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>();
        Range_text = transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>();
        Charge_text = transform.Find("Charge").GetComponentInChildren<TextMeshProUGUI>();

        Range_image = transform.Find("Range").Find("Image").GetComponentInChildren<Image>();
    }

    public override void Initialise(Railgun railgun)
    {
        base.Initialise(railgun);
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(Amenity.Zone);
    }

    protected override void UpdateUI()
    {
        var _damage = Amenity.GetDamageValue();
        var _range = Amenity.GetWeaponRange();
        var _chargingTime = Amenity.GetChargingTimeValue();
        var _timeToChargeConst = Amenity.GetTimeToChargeConst();
        var _chargePercentage = 100.0f * _chargingTime / _timeToChargeConst;
        string _status;
        if (_chargePercentage == 0) _status = "idle";
        else if (_chargePercentage < 100) _status = "charging";
        else _status = "\nready to shoot";

        Status_text.text = $"Status : {_status}";
        Damage_text.text = $"Damage : {_damage}";
        Range_text.text = $"Range : {_range}";
        Charge_text.text = $"Charge : {_chargePercentage.ToString("0.##\\%")}";

        Range_image.color = RangeColors.GetColorForDistance(_range);
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