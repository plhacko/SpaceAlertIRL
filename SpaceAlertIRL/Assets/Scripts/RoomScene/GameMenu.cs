using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject[] OnlyHost;

    private void OnEnable()
    {
        foreach (var go in OnlyHost)
        {
            go.SetActive(NetworkManager.Singleton.IsServer);
        }
    }
}
