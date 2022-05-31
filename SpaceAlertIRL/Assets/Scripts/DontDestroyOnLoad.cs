using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DontDestroyOnLoad : MonoBehaviour
{

    void Awake()
    {
        // DontDestroyOnLoad(this.gameObject); // TODO: rm
        NetworkManager.DontDestroyOnLoad(this.gameObject);
    }

}
