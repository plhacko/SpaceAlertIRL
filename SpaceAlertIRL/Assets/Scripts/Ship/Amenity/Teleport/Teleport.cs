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
        if (!Room.EnergySource.PullEnergy(EnergyCostConst))
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        Player.GetLocalPlayer()?.RequestChangingRoom("Teleport", conectToPanel: false, ignoreRestrictions: true);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("youHaveBeenTeleported_r", clientId: clientId);
    }
}
