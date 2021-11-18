using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingColor : MonoBehaviour
{
    ServerDataContainer ServerDataContainer;

    // Start is called before the first frame update
    void Start()
    {
        ServerDataContainer = GameObject.Find("ServerDataContainer").GetComponent<ServerDataContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OneTimeChangeBackgroudColor()
    {
        ServerDataContainer.OneTimeChangeBackgroundColorServerRpc();
    }
}
