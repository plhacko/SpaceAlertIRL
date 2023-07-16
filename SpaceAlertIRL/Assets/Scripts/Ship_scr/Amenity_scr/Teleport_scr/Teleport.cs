using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Teleport : Amenity<Teleport>
{
    const int EnergyCostConst = 2;
    public int GetEnergyCost() => EnergyCostConst;


    public void RequestTeleportingPlayer()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId; // for the feedback
        TeleportPlayerServerRpc(clientId);
    }

    public override void Restart() { }

    [ServerRpc(RequireOwnership = false)]
    void TeleportPlayerServerRpc(ulong clientId)
    {
        // request energy
        if (Room.EnergySource.PullEnergy(EnergyCostConst)) // success
        {
            Player.GetLocalPlayer()?.RequestChangingRoom("Teleport", ignoreRestrictions: true);
            
            // feedback
            AudioManager.Instance.RequestPlayingSentenceOnClient("youHaveBeenTeleported_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
        }
        else //fail
        {
            // feedback
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
        }
    }
}
