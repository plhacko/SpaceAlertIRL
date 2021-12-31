#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.NetworkVariable;

public class PowerGenerator : EnergyPool
{
    const int EnergyPowerCellStartCountConst = 3;

    public NetworkVariable<int> EnergyPowerCellCount;

    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<PowerGeneratorIcon>().Initialise(this);
    }

#if (SERVER)
    public override void GetEnergy()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (EnergyPowerCellCount.Value > 0)
        {
            EnergyPowerCellCount.Value--;
            EnergyStorage.Value = MaxEnergyStorageConst;
        }
    }
#endif

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        EnergyPowerCellCount = new NetworkVariable<int>(EnergyPowerCellStartCountConst);
    }

}
