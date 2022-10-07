#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerGenerator : EnergyPool
{
    const int EnergyPowerCellStartCountConst = 3;

    public NetworkVariable<int> EnergyPowerCellCount;

    // this is needed because otherwise it would spawn EnergyPoolIcon, not PowerGeneratorIcon
    // (EnergyNode doesn't need it because it is derived from Amenity<EnergyNode>)
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

    [ServerRpc(RequireOwnership = false)]
    public void RequestBurningPowerCellServerRpc()
    {
        GetEnergy();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        EnergyPowerCellCount = new NetworkVariable<int>(EnergyPowerCellStartCountConst);

        UIActions.AddOnValueChangeDependency(EnergyPowerCellCount);
    }
    public override void Restart()
    {
        base.Restart();
        EnergyPowerCellCount.Value = EnergyPowerCellStartCountConst;
    }
}
