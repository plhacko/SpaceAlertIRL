using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Teleport : Amenity<Teleport>
{
    const int EnergyCostConst = 3;
    public int GetEnergyCost() => EnergyCostConst;


    public void RequestTeleportingPlayer()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId; // for the feedback
        TeleportPlayerServerRpc(clientId);
    }

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


        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == clientId)
            {
                player.RequestChangingRoom("Teleport", false);
                return;
            }
        }
    }

}
