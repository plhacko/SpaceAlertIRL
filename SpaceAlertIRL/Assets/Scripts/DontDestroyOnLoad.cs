using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        NetworkManager.DontDestroyOnLoad(this.gameObject);
    }
}
