using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

/// <summary>
/// in order to the ServerFixedUpdate method to be called the object must be added to the static ListToUpdate on ServerUpdater class
/// "ServerUpdater.Add(this);"
/// </summary>
public interface IOnServerFixedUpdate
{
    void ServerFixedUpdate();
}

public class ServerUpdater : NetworkBehaviour
{
    [SerializeField]
    bool Stop = false;

    public void StopUpdating() { Stop = false; }
    public void ResumeUpdating() { Stop = true; }

    static List<IOnServerFixedUpdate> ListToUpdate = new List<IOnServerFixedUpdate>();
    public static void Add(IOnServerFixedUpdate i)
    {
        if (NetworkManager.Singleton.IsServer)
        { ListToUpdate.Add(i); }
    }

    void FixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; }
        if (Stop) { return; }

        if (SceneManager.GetActiveScene().name != "RoomScene") { return; } // TODO: rethink and make it a optimal solution

        ListToUpdate.RemoveAll(item => item == null);
        foreach (var i in ListToUpdate)
        {
            i.ServerFixedUpdate();
        }
    }
}

