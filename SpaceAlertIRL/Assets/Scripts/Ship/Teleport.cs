using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Teleport : Amenity<Teleport>
{
    const int teleportationCost = 3;

    public void RequestTeleportingPlayer()
    {
        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                player.RequestChangingRoom("Teleport", false);
                return;
            }
        }
    }

}
