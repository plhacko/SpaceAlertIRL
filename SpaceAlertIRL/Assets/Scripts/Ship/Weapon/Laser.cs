using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Laser : Weapon
{
    const int DamageConst = 5;
    const int RangeConst = 4;
    const float StartHeatConst = 0.0f;

    public NetworkVariable<int> Damage;
    public NetworkVariable<int> Range;
    public NetworkVariable<float> Heat;

    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<LaserIcon>().Initialise(this);
    }


    protected override void Start()
    {
        base.Start();

        Damage = new NetworkVariable<int>(DamageConst);
        Range = new NetworkVariable<int>(RangeConst);
        Heat = new NetworkVariable<float>(StartHeatConst);

        UIActions.AddOnValueChangeDependency(Damage);
        UIActions.AddOnValueChangeDependency(Range);
        UIActions.AddOnValueChangeDependency(Heat);
    }
}
