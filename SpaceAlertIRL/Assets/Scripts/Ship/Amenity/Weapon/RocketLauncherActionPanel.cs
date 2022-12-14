using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketLauncherActionPanel : AmenityActionPanel<RocketLauncher>
{
    [SerializeField] // TODO: rm
    private int TargetableZoneNamesIndex = 0;

    public override void Initialise(RocketLauncher rl)
    {
        base.Initialise(rl);

        // target zone is chozen based on the "TargetableZoneNamesIndex"
        ZoneNames _zoneName = Amenity.TagrgetableZoneNames[TargetableZoneNamesIndex];
        Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(_zone);
    }

    protected override void UpdateUI()
    {
        Rocket rocket = Amenity.RocketPrefab.GetComponent<Rocket>();

        var _damage = rocket.Damage;
        var _range = rocket.Range;
        var _rocketCount = Amenity.NumberOfRockets.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good";
        transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>().text = $"Damage : {_damage}";
        transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>().text = $"Range : {_range.ToString("0.00")}";
        transform.Find("RocketCount").GetComponentInChildren<TextMeshProUGUI>().text = $"Rocket Count : {_rocketCount}";

        transform.Find("TargetedZoneName").GetComponentInChildren<TextMeshProUGUI>().text = Amenity.TagrgetableZoneNames[TargetableZoneNamesIndex].ToString();
    }

    public void ChangeTargetedZone_moveLeft()
    {
        if (TargetableZoneNamesIndex > 0)
        {
            TargetableZoneNamesIndex--;

            ZoneNames _zoneName = Amenity.TagrgetableZoneNames[TargetableZoneNamesIndex];
            Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
            GetComponentInChildren<EnemyIconSpawner>().ChangeZone(_zone);

            UpdateUI();
        }
    }

    public void ChangeTargetedZone_moveRight()
    {
        if (Amenity.TagrgetableZoneNames.Length - 1 > TargetableZoneNamesIndex)
        {
            TargetableZoneNamesIndex++;

            ZoneNames _zoneName = Amenity.TagrgetableZoneNames[TargetableZoneNamesIndex];
            Zone _zone = GameObject.Find(_zoneName.ToString()).GetComponent<Zone>();
            GetComponentInChildren<EnemyIconSpawner>().ChangeZone(_zone);

            UpdateUI();
        }
    }
    public void RequestShootingRocket()
    {
        Amenity.RequestLaunchingRocket(Amenity.TagrgetableZoneNames[TargetableZoneNamesIndex]); // TODO: remove constant
    }
}
