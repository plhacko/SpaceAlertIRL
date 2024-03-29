using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RailgunActionPanel : AmenityActionPanel<Railgun>
{
    TextMeshProUGUI Status_text, Damage_text, Range_text, Charge_text;
    Image Range_image, ShootButton, ChargeButton;

    private void Awake()
    {
        Status_text = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Damage_text = transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>();
        Range_text = transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>();
        Charge_text = transform.Find("Charge").GetComponentInChildren<TextMeshProUGUI>();

        Range_image = transform.Find("Range").Find("Image").GetComponentInChildren<Image>();
        ShootButton = transform.Find("ShootButton").GetComponent<Image>();
        ChargeButton = transform.Find("ChargeButton").GetComponent<Image>();
    }

    public override void Initialise(Railgun railgun)
    {
        base.Initialise(railgun);
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(Amenity.Zone);
    }

    protected override void UpdateUI()
    {
        var _damage = Amenity.Damage;
        var _range = Amenity.Range;
        var _chargingTime = Amenity.ChargingTime;
        var _timeToChargeConst = Amenity.TimeToCharge;
        var _chargePercentage = 100.0f * _chargingTime / _timeToChargeConst;
        string _status;
        if (_chargePercentage == 0) _status = "idle";
        else if (_chargePercentage < 100) _status = "charging";
        else _status = "\nready to shoot";

        Status_text.text = $"Status : {_status}";
        Damage_text.text = $"Damage : {_damage}";
        Range_text.text = $"Range : {_range}";
        Charge_text.text = $"Charge : {_chargePercentage.ToString("0.##\\%")}";

        Range_image.color = ProjectColors.GetColorForDistance(_range);


        Color c;
        c = ShootButton.color;
        c.a = Amenity.IsCharged ? 1f : 0.6f;
        ShootButton.color = c;

        c = ChargeButton.color;
        c.a = Amenity.IsCharged ? 0.6f : 1f;
        ChargeButton.color = c;
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