using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

interface IOnServerFixedUpdate
{
    void ServerFixedUpdate();
}

public class ServerUpdater : NetworkBehaviour
{
    void FixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; } //TODO: rm

        if (SceneManager.GetActiveScene().name != "RoomScene") { return; } // TODO: rethink and make it a optimal solution

        var arrayToUpdate = GetComponentsInChildren<IOnServerFixedUpdate>();
        foreach (var i in arrayToUpdate)
        {
            i.ServerFixedUpdate();
        }
    }
}

