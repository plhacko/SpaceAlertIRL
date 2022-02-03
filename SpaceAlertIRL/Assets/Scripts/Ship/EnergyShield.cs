using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnergyShield : Amenity
{
    protected const int MaxShieldValueConst = 3;
    protected const int ShieldValueConst = 2;

    public NetworkVariable<int> MaxShieldValue;
    public NetworkVariable<int> ShieldValue;

    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<EnergyShieldIcon>().Initialise(this);
    }

    protected override void Start()
    {
        base.Start();

        ShieldValue = new NetworkVariable<int>(ShieldValueConst);
        MaxShieldValue = new NetworkVariable<int>(MaxShieldValueConst);

        UIActions.AddOnValueChangeDependency(ShieldValue);
        UIActions.AddOnValueChangeDependency(MaxShieldValue);
    }
}
