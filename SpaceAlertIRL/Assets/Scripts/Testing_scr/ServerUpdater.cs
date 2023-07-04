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
    static bool Stop = false;
    static public void StopUpdating() { Stop = false; }
    static public void ResumeUpdating() { Stop = true; }

    static List<GameObject> ListToUpdate = new List<GameObject>();
    static List<GameObject> NewlyAddedListToUpdate = new List<GameObject>();
    static List<GameObject> NewlyRemovedListToUpdate = new List<GameObject>();
    public static void Add(GameObject i)
    {
        if (i.GetComponent<IOnServerFixedUpdate>() == null) { throw new System.Exception("GameObject must contain script with IOnServerFixedUpdate interface"); }
        NewlyAddedListToUpdate.Add(i);
    }
    public static void Remove(GameObject i)
    {
        if (!ListToUpdate.Contains(i)) { throw new System.Exception("ListToUpdate must contain the object"); }
        NewlyRemovedListToUpdate.Add(i);
    }

    void FixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; }
        if (Stop) { return; }

        // add things to update
        ListToUpdate.AddRange(NewlyAddedListToUpdate);
        NewlyAddedListToUpdate = new List<GameObject>();

        if (SceneManager.GetActiveScene().name != "RoomScene") { return; } // TODO: rethink and make it a optimal solution

        ListToUpdate.RemoveAll(item => item == null);
        foreach (var i in ListToUpdate)
        {
            i.GetComponent<IOnServerFixedUpdate>().ServerFixedUpdate();
        }
    }
}

