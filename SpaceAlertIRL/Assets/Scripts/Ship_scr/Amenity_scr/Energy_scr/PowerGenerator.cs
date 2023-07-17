#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerGenerator : EnergyPool
{
    const int EnergyPowerCellStartCountConst = 3;

    public NetworkVariable<int> _EnergyPowerCellCount;
    public int EnergyPowerCellCount { get => _EnergyPowerCellCount.Value; protected set { _EnergyPowerCellCount.Value = value; } }

    // this is needed because otherwise it would spawn EnergyPoolIcon, not PowerGeneratorIcon
    // (EnergyNode doesn't need it because it is derived from Amenity<EnergyNode>)
    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<PowerGeneratorIcon>().Initialise(this);
    }

#if (SERVER)
    internal override void GetEnergy(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (EnergyPowerCellCount > 0 && EnergyStorage < MaxEnergyStorageConst)
        {
            EnergyPowerCellCount--;
            EnergyStorage = MaxEnergyStorageConst;

            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
        }
        else if (EnergyPowerCellCount == 0)
        {
            // message for the player, tahat there was not enough energy
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughPowerCells_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
        }
        else
        { AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId); }
    }
#endif

    [ServerRpc(RequireOwnership = false)]
    public void RequestBurningPowerCellServerRpc(ulong clientId)
    {
        GetEnergy(clientId: clientId);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _EnergyPowerCellCount = new NetworkVariable<int>(EnergyPowerCellStartCountConst);

        UIActions.AddOnValueChangeDependency(_EnergyPowerCellCount);
    }
    public override void Restart()
    {
        base.Restart();
        EnergyPowerCellCount = EnergyPowerCellStartCountConst;
    }
}
