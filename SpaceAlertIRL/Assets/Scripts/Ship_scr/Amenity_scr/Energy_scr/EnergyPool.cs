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

    NetworkVariable<int> _MaxEnergyStorage;
    public NetworkVariable<int> _EnergyStorage;

    public int MaxEnergyStorage { get => _MaxEnergyStorage.Value; protected set { _MaxEnergyStorage.Value = value; } }
    public int EnergyStorage { get => _EnergyStorage.Value; protected set { _EnergyStorage.Value = value; } }

    // UI 
    BubbleProgressBar BubbleProgressBar;

    // this is needed because otherwise it would spawn EnergyNodeIcon, not EnergyPoollIcon
    // (EnergyNode doesn't need it because it is derived from Amenity<EnergyNode>)
    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<EnergyPoolIcon>().Initialise(this);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestEnergyTransferServerRpc(ulong clientId)
    {
        GetEnergy(clientId: clientId);
    }

    // those methods should be called only by server
#if (SERVER)
    public virtual void GetEnergy(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        var energyRequest = MaxEnergyStorageConst - AvailableEnergy();
        var pulledEnergy = Source.PullEnergyUpTo(energyRequest);

        EnergyStorage += pulledEnergy;

        // audio feedback
        if (pulledEnergy > 0)
        { AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId); }
        else
        {
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.error, clientId: clientId);
        }
    }

    public override bool PullEnergy(int amount)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (EnergyStorage >= amount)
        {
            EnergyStorage -= amount;
            return true;
        }
        else
        { return false; }
    }

    public override int PullEnergyUpTo(int amount)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (EnergyStorage >= amount)
        {
            EnergyStorage -= amount;
            return amount;
        }
        else
        {
            int _tmp = EnergyStorage;
            EnergyStorage = 0;
            return _tmp;
        }
    }

    public override int AvailableEnergy()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }
        return EnergyStorage;
    }
#endif


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _EnergyStorage = new NetworkVariable<int>(StartingEnergyStorageConst);
        _MaxEnergyStorage = new NetworkVariable<int>(MaxEnergyStorageConst);

        // UI
        BubbleProgressBar = GetComponent<BubbleProgressBar>();

        UIActions.AddOnValueChangeDependency(_EnergyStorage, _MaxEnergyStorage);

        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();
    }

    public override void Restart()
    {
        base.Restart();

        EnergyStorage = StartingEnergyStorageConst;
        MaxEnergyStorage = MaxEnergyStorageConst;
    }

    void UpdateUI()
    {
        // shows visualy how much energy is being stored
        BubbleProgressBar?.UpdateUI(EnergyStorage, MaxEnergyStorageConst);
    }
}
