#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PowerGenerator : EnergyPool
{
    const int EnergyPowerCellStartCountConst = 3;

    public NetworkVariableInt EnergyPowerCellCount;

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

        EnergyPowerCellCount = new NetworkVariableInt(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        }, EnergyPowerCellStartCountConst);
        
    }



    // Update is called once per frame
    void Update()
    {

    }
}
