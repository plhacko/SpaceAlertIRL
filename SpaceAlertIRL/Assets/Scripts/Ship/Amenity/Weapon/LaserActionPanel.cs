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

    TextMeshProUGUI Status_text;
    TextMeshProUGUI Damage_text;
    TextMeshProUGUI Range_text;
    TextMeshProUGUI Heat_text;

    Image Range_image;

    private void Awake()
    {
        Status_text = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Damage_text = transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>();
        Range_text = transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>();
        Heat_text = transform.Find("Heat").GetComponentInChildren<TextMeshProUGUI>();

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
        var _range = Amenity.GetWeaponRange();
        var _heat = Amenity.GetWeaponHeat();
        bool _tooHot = Amenity.IsTooHotToShoot();
        string _statusTest = _tooHot ? "high heat" : "ok";

        Status_text.text = $"Status : {_statusTest}";
        Damage_text.text = $"Damage : {_damage}";
        Range_text.text = $"Range : {_range}";
        Heat_text.text = $"Heat : {_heat.ToString("0.00\\%")}";

        Range_image.color = RangeColors.GetColorForDistance(_range);

        if (_tooHot && !Amenity.IsActivelyCooled())
        { ActiveCoolingButton.SetActive(true); }
        else
        { ActiveCoolingButton.SetActive(false); }
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
