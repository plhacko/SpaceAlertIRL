using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using UnityEngine.UI;

public class RocketLauncherActionPanel : AmenityActionPanel<RocketLauncher>
{
    TextMeshProUGUI Status_text, Damage_text, Range_text, RocketCount_text, TargetedZoneName_text;
    Image Range_image, ShootButton;

    private void Awake()
    {
        Status_text = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Damage_text = transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>();
        Range_text = transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>();
        RocketCount_text = transform.Find("RocketCount").GetComponentInChildren<TextMeshProUGUI>();
        TargetedZoneName_text = transform.Find("TargetedZoneName").GetComponentInChildren<TextMeshProUGUI>();

        Range_image = transform.Find("Range").Find("Image").GetComponentInChildren<Image>();
        ShootButton = transform.Find("ShootButton").GetComponent<Image>();
    }

    public override void Initialise(RocketLauncher rl)
    {
        base.Initialise(rl);

        // target zone is chozen based on the "TargetedZone"
        ZoneNames _zoneName = Amenity.TargetableZoneNames[(int)rl.TargetedZone];
        Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(_zone);
    }

    protected override void UpdateUI()
    {
        Rocket rocket = Amenity.RocketPrefab.GetComponent<Rocket>();

        var _damage = rocket.Damage;
        var _range = rocket.Range;
        var _rocketCount = Amenity.NumberOfRockets;
        var _status = _rocketCount > 0 ? "\nready to shoot" : "\nout of rockets";

        Damage_text.text = $"Damage : {_damage}";
        Range_text.text = $"Range : {_range.ToString()}";
        RocketCount_text.text = $"Rockets : {_rocketCount}";
        TargetedZoneName_text.text = Amenity.TargetableZoneNames[(int)Amenity.TargetedZone].ToString();
        Status_text.text = $"Status : {_status}";

        Range_image.color = ProjectColors.GetColorForDistance(_range);

        Color c = ShootButton.color;
        c.a = Amenity.NumberOfRockets > 0 ? 1f : 0.6f;
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
    public void RequestShootingRocket()
    {
        Amenity.RequestLaunchingRocket(Amenity.TargetableZoneNames[(int)Amenity.TargetedZone]);
    }
}
