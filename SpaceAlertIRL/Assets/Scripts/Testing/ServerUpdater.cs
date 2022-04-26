using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

interface IOnServerFixedUpdate
{
    void ServerFixedUpdate();
}

public class ServerUpdater : NetworkBehaviour
{
    void FixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; } //TODO: rm
        var arrayToUpdate = GetComponentsInChildren<IOnServerFixedUpdate>();
        foreach (var i in arrayToUpdate)
        {
            i.ServerFixedUpdate();
        }
    }
}

