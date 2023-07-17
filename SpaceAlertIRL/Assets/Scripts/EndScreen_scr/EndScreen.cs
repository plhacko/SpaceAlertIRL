using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    GameObject Victory;
    [SerializeField]
    GameObject Defeat;

    [SerializeField]
    GameObject[] OnlyHost;
    [SerializeField]
    GameObject[] OnlyClient;

    void Start()
    {
        if (IsDead())
        {
            Victory.SetActive(false);
            Defeat.SetActive(true);
        }
        else
        {
            Victory.SetActive(true);
            Defeat.SetActive(false);
        }

        foreach (GameObject go in OnlyHost)
        { go.SetActive(NetworkManager.Singleton.IsServer); }
        foreach (GameObject go in OnlyClient)
        { go.SetActive(!NetworkManager.Singleton.IsServer); }
    }

    private bool IsDead()
    {
        Zone[] Zones = GameObject.Find("ShipCanvas").GetComponentsInChildren<Zone>();

        foreach (Zone z in Zones)
        {
            if (z.HP <= 0)
            { return true; }
        }
        return false;
    }
}
