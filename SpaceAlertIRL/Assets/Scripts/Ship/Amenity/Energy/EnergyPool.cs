#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class EnergyPool : EnergyNode
{
    public const int MaxEnergyStorageConst = 5;
    protected const int StartingEnergyStorageConst = 4;

    public NetworkVariable<int> MaxEnergyStorage;
    public NetworkVariable<int> EnergyStorage;

    // UI
    [SerializeField] GameObject EnergyCircle_full_prefab;
    [SerializeField] GameObject EnergyCircle_empty_prefab;

    // this is needed because otherwise it would spawn EnergyNodeIcon, not EnergyPoollIcon
    // (EnergyNode doesn't need it because it is derived from Amenity<EnergyNode>)
    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<EnergyPoolIcon>().Initialise(this);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestEnergyTransferServerRpc()
    {
        GetEnergy();
    }

    // hose methods should be called only by server
#if (SERVER)
    public virtual void GetEnergy()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        var energyRequest = MaxEnergyStorageConst - AvailableEnergy();
        var pulledEnergy = Source.PullEnergyUpTo(energyRequest);

        EnergyStorage.Value += pulledEnergy;
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

        EnergyStorage = new NetworkVariable<int>(StartingEnergyStorageConst);
        MaxEnergyStorage = new NetworkVariable<int>(MaxEnergyStorageConst);

        UIActions.AddOnValueChangeDependency(EnergyStorage, MaxEnergyStorage);

        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();
    }

    public override void Restart()
    {
        base.Restart();

        EnergyStorage.Value = StartingEnergyStorageConst;
        MaxEnergyStorage.Value = MaxEnergyStorageConst;
    }

    void UpdateUI()
    {
        // reset self
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // spawn energy circles
        for (int i = 0; i < EnergyStorage.Value; i++)
        {
            Instantiate(EnergyCircle_full_prefab, parent: transform);
        }
        for (int j = EnergyStorage.Value; j < MaxEnergyStorageConst; j++)
        {
            Instantiate(EnergyCircle_empty_prefab, parent: transform);
        }
    }
}
