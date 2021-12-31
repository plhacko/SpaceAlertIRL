#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.Messaging;

public class EnergyNode : Amenity
{
    [SerializeField]
    protected EnergyNode Source;

    public override void SpawnIconAsChild(GameObject parent)
    {
        throw new System.NotImplementedException();
    }

#if (SERVER)
    // is used to detect circles
    protected bool IsOnEnergyPath = false;

    public virtual bool PullEnergy(int amount)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (IsOnEnergyPath || Source == null)
        { return false; }

        IsOnEnergyPath = true;
        bool result = Source.PullEnergy(amount);
        IsOnEnergyPath = false;
        return result;
    }

    public virtual int PullEnergyUpTo(int amount)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (IsOnEnergyPath || Source == null)
        { return 0; }

        IsOnEnergyPath = true;
        int result = Source.PullEnergyUpTo(amount);
        IsOnEnergyPath = false;
        return result;
    }

    public virtual int AvailableEnergy()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (IsOnEnergyPath || Source == null)
        { return 0; }

        IsOnEnergyPath = true;
        int result = Source.AvailableEnergy();
        IsOnEnergyPath = false;
        return result;
    }
#endif
    protected override void Start()
    {
        base.Start();
    }
}
