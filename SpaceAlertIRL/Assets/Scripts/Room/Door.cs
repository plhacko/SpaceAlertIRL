#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.NetworkVariable;
//rm using MLAPI.Messaging;
using System;



public class Door : NetworkBehaviour
{
    [SerializeField]
    float OpenningSpeedConst = 0.2f;

    public string Name { get => gameObject.name; }

    public Room RoomA;
    public Room RoomB;

    public string Status
    { //TODO: need to be finished up -> open/slosed/...
        get
        {
            if (IsOpen.Value)
            {
                if (OpenningClosingProgress.Value >= 1) return "open";
                else return "closing";
            }
            else
            {
                if (OpenningClosingProgress.Value >= 1) return "closed";
                else return "opening";
            }
        }
    }

    // old
    // public ActionNetworkVariable<bool> IsOpen = new ActionNetworkVariable<bool>(new NetworkVariableBool(new NetworkVariableSettings
    // {
    //     WritePermission = NetworkVariablePermission.ServerOnly,
    //     ReadPermission = NetworkVariablePermission.Everyone
    // }, false));

    public NetworkVariable<bool> IsOpen;
    public UpdateUIActions IsOpenUIActions;

    // todo: rename this
    public NetworkVariable<float> OpenningClosingProgress;
    public UpdateUIActions OpenningClosingProgressUIActions;

    // Start is called before the first frame update
    void Start()
    {
        AddSelfToRoom(RoomA);
        AddSelfToRoom(RoomB);

        IsOpen = new NetworkVariable<bool>(false);
        IsOpenUIActions = new UpdateUIActions();
        IsOpenUIActions.AddOnValueChangeDependency(IsOpen);

        OpenningClosingProgress = new NetworkVariable<float>(1.0f);
        OpenningClosingProgressUIActions = new UpdateUIActions();
        OpenningClosingProgressUIActions.AddOnValueChangeDependency(OpenningClosingProgress);
    }


#if (SERVER)
    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (OpenningClosingProgress.Value < 1)
            {
                float _currentOpenning = OpenningClosingProgress.Value + OpenningSpeedConst * Time.deltaTime;
                if (_currentOpenning > 1)
                {
                    IsOpen.Value = !IsOpen.Value;
                    OpenningClosingProgress.Value = 1;
                }
                else { OpenningClosingProgress.Value = _currentOpenning; }
            }
            // Debug.Log(OpenningClosingProgress.Value); // TODO: delete
        }
    }
#endif

    // TODO: make openning and closing its own process
    // TODO: check if the player is in the right room
    [ServerRpc(RequireOwnership = false)]
    public void OpenDoorServerRpc(ServerRpcParams rpcParams = default)
    {
        if (!IsOpen.Value)
            OpenningClosingProgress.Value = 0.0f;
        // old
        // IsOpen.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CloseDoorServerRpc(ServerRpcParams rpcParams = default)
    {
        if (IsOpen.Value)
            OpenningClosingProgress.Value = 0.0f;
        // old
        // IsOpen.Value = false;
    }

    void AddSelfToRoom(Room r)
    {
        r.AddDoor(this);
    }

    public bool IsConnectedToRoom(Room r) // TODO: think about if this should be server RPC? -> ir would be dificult to make it so and it is most likely not needed // todo: more thinking
    {
        return r == RoomA || r == RoomB;
    }
}
