using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnSomething : MonoBehaviour
{
    [SerializeField]
    GameObject NetworkObjectToSpawn;

    public void InitializeAndSpawnObject()
    {
        GameObject go = Instantiate(NetworkObjectToSpawn);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
