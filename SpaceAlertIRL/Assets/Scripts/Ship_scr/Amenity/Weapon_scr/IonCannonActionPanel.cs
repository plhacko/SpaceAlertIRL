using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IonCannonActionPanel : AmenityActionPanel<IonCannon>
{
    TextMeshProUGUI Status_text, Damage_text, Range_text, Heat_text, EnergyCost_text, TargetedZoneName_text;

    Image Range_image, ShootButton;

    private void Awake()
    {
        Status_text = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Damage_text = transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>();
        Range_text = transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>();
        Heat_text = transform.Find("Heat").GetComponentInChildren<TextMeshProUGUI>();
        EnergyCost_text = transform.Find("EnergyCost").GetComponentInChildren<TextMeshProUGUI>();
        TargetedZoneName_text = transform.Find("TargetedZoneName").GetComponentInChildren<TextMeshProUGUI>();

        Range_image = transform.Find("Range").Find("Image").GetComponentInChildren<Image>();
        ShootButton = transform.Find("ShootButton").GetComponent<Image>();
    }

    public override void Initialise(IonCannon ionCannon)
    {
        base.Initialise(ionCannon);
        // target zone is chozen based on the "TargetedZone"
        ZoneNames _zoneName = Amenity.TargetableZoneNames[(int)ionCannon.TargetedZone];
        Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(_zone);
    }

    protected override void UpdateUI()
    {
        var _damage = Amenity.Damage;
        var _energyCost = Amenity.EnergyCost;
        var _range = Amenity.Range;
        var _heat = Amenity.Heat;
        bool _isTooHot = Amenity.IsTooHotToShoot;
        string _status = _isTooHot ? "cooling" : "ready";

        Damage_text.text = $"Depletes up to {_damage} energy in all ships";
        EnergyCost_text.text = $"Energy cost : {_energyCost}";
        Range_text.text = $"Range : {_range}";
        Heat_text.text = $"Heat : {_heat.ToString("0.0")}%";
        Status_text.text = $"Status : {_status}";
        TargetedZoneName_text.text = Amenity.TargetableZoneNames[(int)Amenity.TargetedZone].ToString();

        Range_image.color = ProjectColors.GetColorForDistance(_range);

        Color c = ShootButton.color;
        c.a = _isTooHot ? 0.6f : 1f;
        ShootButton.color = c;
    }

    public void ChangeTargetedZone_moveLeft()
    {
        if (Amenity.TargetedZone > 0)
        {
            Amenity.TargetedZone--;

            ZoneNames _zoneName = Amenity.TargetableZoneNames[(int)Amenity.TargetedZone];
            Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
            GetComponentInChildren<EnemyIconSpawner>().ChangeZone(_zone);

            UpdateUI();
        }
    }

    public void ChangeTargetedZone_moveRight()
    {
        if (Amenity.TargetableZoneNames.Length - 1 > (int)Amenity.TargetedZone)
        {
            Amenity.TargetedZone++;

            ZoneNames _zoneName = Amenity.TargetableZoneNames[(int)Amenity.TargetedZone];
            Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
            GetComponentInChildren<EnemyIconSpawner>().ChangeZone(_zone);

            UpdateUI();
        }
    }

    public void RequestShootingAtEnemies()
    {
        Amenity.RequestShootingAtEnemies();
    }
}