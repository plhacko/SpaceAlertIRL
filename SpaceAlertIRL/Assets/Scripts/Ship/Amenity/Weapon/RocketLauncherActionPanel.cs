using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using UnityEngine.UI;

public class RocketLauncherActionPanel : AmenityActionPanel<RocketLauncher>
{
    TextMeshProUGUI Status_text;
    TextMeshProUGUI Damage_text;
    TextMeshProUGUI Range_text;

    Image Range_image;

    private void Start()
    {
        Status_text = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Damage_text = transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>();
        Range_text = transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>();

        Range_image = transform.Find("Range").Find("Image").GetComponentInChildren<Image>();
    }

    public override void Initialise(RocketLauncher rl)
    {
        base.Initialise(rl);

        // target zone is chozen based on the "TargetedZone"
        ZoneNames _zoneName = Amenity.TagrgetableZoneNames[(int)rl.TargetedZone];
        Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(_zone);

    }

    protected override void UpdateUI()
    {
        Rocket rocket = Amenity.RocketPrefab.GetComponent<Rocket>();

        var _damage = rocket.Damage;
        var _range = rocket.Range;
        var _rocketCount = Amenity.NumberOfRockets;
        var _status = _rocketCount > 0 ? "Status : iddle" : "Out of rockets";

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = $"{_status}";
        transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>().text = $"Damage : {_damage}";
        transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>().text = $"Range : {_range.ToString()}";
        transform.Find("RocketCount").GetComponentInChildren<TextMeshProUGUI>().text = $"Rockets : {_rocketCount}";

        transform.Find("TargetedZoneName").GetComponentInChildren<TextMeshProUGUI>().text = Amenity.TagrgetableZoneNames[(int)Amenity.TargetedZone].ToString();
    }

    public void ChangeTargetedZone_moveLeft()
    {
        if (Amenity.TargetedZone > 0)
        {
            Amenity.TargetedZone--;

            ZoneNames _zoneName = Amenity.TagrgetableZoneNames[(int)Amenity.TargetedZone];
            Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
            GetComponentInChildren<EnemyIconSpawner>().ChangeZone(_zone);

            UpdateUI();
        }
    }

    public void ChangeTargetedZone_moveRight()
    {
        if (Amenity.TagrgetableZoneNames.Length - 1 > (int)Amenity.TargetedZone)
        {
            Amenity.TargetedZone++;

            ZoneNames _zoneName = Amenity.TagrgetableZoneNames[(int)Amenity.TargetedZone];
            Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
            GetComponentInChildren<EnemyIconSpawner>().ChangeZone(_zone);

            UpdateUI();
        }
    }
    public void RequestShootingRocket()
    {
        Amenity.RequestLaunchingRocket(Amenity.TagrgetableZoneNames[(int)Amenity.TargetedZone]);
    }
}
