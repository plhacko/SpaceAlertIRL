using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnShip : MonoBehaviour
{

    [SerializeField]
    GameObject Ship;

    public void Spawn()
    {
        GameObject go = Instantiate(Ship, Vector3.zero, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();

        foreach (var i in go.GetComponentsInChildren<NetworkObject>())
        {
            if (!i.IsSpawned)
            { i.Spawn(); }
        }
    }
}
