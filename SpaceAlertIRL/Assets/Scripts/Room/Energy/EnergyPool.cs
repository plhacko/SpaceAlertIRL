#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using System;

public class EnergyPool : EnergyNode
{
    protected const int MaxEnergyStorageConst = 5;

    public NetworkVariableInt MaxEnergyStorage;
    public NetworkVariableInt EnergyStorage;
    

// hose methods should be called only by server
#if(SERVER)
    public virtual void GetEnergy()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        EnergyStorage.Value = PullEnergyUpTo(MaxEnergyStorageConst);
    }

    public override bool PullEnergy(int amount)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (EnergyStorage.Value >= amount)
        {
            EnergyStorage.Value -= amount;
            return true;
        }
        else
        { return false; }
    }

    public override int PullEnergyUpTo(int amount)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (EnergyStorage.Value >= amount)
        {
            EnergyStorage.Value -= amount;
            return amount;
        }
        else
        {
            int _tmp = EnergyStorage.Value;
            EnergyStorage.Value = 0;
            return _tmp;
        }
    }

    // TODO: think -> if user uses this it should not matter
    public override int AvailableEnergy()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }
        return EnergyStorage.Value;
    }
#endif


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        EnergyStorage = new NetworkVariableInt(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        }, MaxEnergyStorageConst);

        UIActions.AddOnValueChangeDependency(EnergyStorage);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
