using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class ServerDataContainer : NetworkBehaviour
{
    public NetworkVariableString myString = new NetworkVariableString("testText");
    public NetworkVariableInt myInt = new NetworkVariableInt(WritePermission = , 0);


   
    [ServerRpc(RequireOwnership = false)]
    public void AddServerRpc(ServerRpcParams rpcParams = default)
    {
        print("DEBUG: middleStartTime-" + Time.time.ToString());
        myInt.Value++;
        print("DEBUG: middleEndTime-" + Time.time.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
