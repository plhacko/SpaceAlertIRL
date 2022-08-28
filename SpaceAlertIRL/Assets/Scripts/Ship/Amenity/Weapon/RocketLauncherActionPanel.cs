using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketLauncherActionPanel : AmenityActionPanel<RocketLauncher>
{
    public override void Initialise(RocketLauncher rl)
    {
        base.Initialise(rl);
        transform.GetComponentInChildren<EnemyIconSpawner>().Initialise(Amenity.Zone);
    }

    protected override void UpdateUI()
    {
        Rocket rocket = Amenity.RocketPrefab.GetComponent<Rocket>();

        var _damage = rocket.Damage;
        var _range = rocket.Range;
        var _rocketCount = Amenity.NumberOfRockets.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>().text = $"Damage : {_damage}";
        transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>().text = $"Range : {_range.ToString("0.00")}";
        transform.Find("RocketCount").GetComponentInChildren<TextMeshProUGUI>().text = $"Rocket Count : {_rocketCount}";
    }


    public void RequestShootingRocket()
    {
        Amenity.RequestLaunchingRocket();
    }
}
