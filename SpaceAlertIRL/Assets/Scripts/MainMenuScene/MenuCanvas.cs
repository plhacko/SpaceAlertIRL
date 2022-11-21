using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject HostMenu;
    [SerializeField]
    GameObject ClientMenu;

    void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostMenu.SetActive(true);
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            ClientMenu.SetActive(true);
        }
    }
}
