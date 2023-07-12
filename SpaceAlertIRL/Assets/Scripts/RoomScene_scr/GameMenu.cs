using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject[] OnlyHost;
    [SerializeField] GameObject[] OnlyClient;

    private void OnEnable()
    {
        foreach (var go in OnlyHost)
        {
            go.SetActive(NetworkManager.Singleton.IsServer);
        }

        foreach (var go in OnlyClient)
        {
            go.SetActive(NetworkManager.Singleton.IsClient);
        }
    }


}
