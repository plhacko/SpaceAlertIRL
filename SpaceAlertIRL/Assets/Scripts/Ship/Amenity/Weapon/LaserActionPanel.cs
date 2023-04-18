using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LaserActionPanel : AmenityActionPanel<Laser>
{
    [SerializeField]
    GameObject ActiveCoolingButton;
    [SerializeField]
    Image ShootButton;

    TextMeshProUGUI Status_text, Damage_text, Range_text, Heat_text, EnergyCost_text;

    Image Range_image;

    private void Awake()
    {
        Status_text = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Damage_text = transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>();
        Range_text = transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>();
        Heat_text = transform.Find("Heat").GetComponentInChildren<TextMeshProUGUI>();
        EnergyCost_text = transform.Find("EnergyCost").GetComponentInChildren<TextMeshProUGUI>();

        Range_image = transform.Find("Range").Find("Image").GetComponentInChildren<Image>();
    }

    public override void Initialise(Laser laser)
    {
        base.Initialise(laser);
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(Amenity.Zone);
    }

    protected override void UpdateUI()
    {
        var _damage = Amenity.GetWeaponDamage();
        var _energyCost = Amenity.GetEnergyCost();
        var _range = Amenity.GetWeaponRange();
        var _heat = Amenity.GetWeaponHeat();
        bool _isTooHot = Amenity.IsTooHotToShoot();
        string _status = _isTooHot ? "\ncooling down" : "\nready to shoot";

        Damage_text.text = $"Damage : {_damage}";
        EnergyCost_text.text = $"Energy cost : {_energyCost}";
        Range_text.text = $"Range : {_range}";
        Heat_text.text = $"Heat : {_heat.ToString("0.00\\%")}";
        Status_text.text = $"Status : {_status}";

        Range_image.color = RangeColors.GetColorForDistance(_range);

        if (_isTooHot && !Amenity.IsActivelyCooled())
            ActiveCoolingButton.SetActive(true);
        else
            ActiveCoolingButton.SetActive(false);

        Color c = ShootButton.color;
        c.a = _isTooHot ? 0.6f : 1f;
        ShootButton.color = c;
    }

    public void RequestShootingAtClosesEnemy()
    {
        Amenity.RequestShootingAtClosestEnemy();
    }

    public void RequestActivateActiveCooling()
    {
        Amenity.RequestActivateActiveCooling();
    }
}
