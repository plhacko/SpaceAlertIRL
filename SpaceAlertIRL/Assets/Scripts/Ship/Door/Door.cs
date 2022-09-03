#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;



public class Door : NetworkBehaviour, IOnServerFixedUpdate
{
    [SerializeField]
    float OpenningSpeedConst = 0.5f;

    public string Name { get => gameObject.name; }

    public Room RoomA;
    public Room RoomB;

    public string Status
    { //TODO: need to be finished up -> open/closed/...
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

    public NetworkVariable<bool> IsOpen;
    
    public NetworkVariable<float> OpenningClosingProgress;

    public UpdateUIActions UIActions = new UpdateUIActions();

    void Start()
    {
        AddSelfToRoom(RoomA);
        AddSelfToRoom(RoomB);

        IsOpen = new NetworkVariable<bool>(false);
        UIActions.AddOnValueChangeDependency(IsOpen);

        OpenningClosingProgress = new NetworkVariable<float>(1.0f);
        UIActions.AddOnValueChangeDependency(OpenningClosingProgress);
    }


#if (SERVER)
    public void ServerFixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer)
        { return; }

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
    }
#endif

    [ServerRpc(RequireOwnership = false)]
    public void OpenDoorServerRpc(ServerRpcParams rpcParams = default)
    {
        if (!IsOpen.Value)
            OpenningClosingProgress.Value = 0.0f;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CloseDoorServerRpc(ServerRpcParams rpcParams = default)
    {
        if (IsOpen.Value)
            OpenningClosingProgress.Value = 0.0f;
    }

    void AddSelfToRoom(Room r)
    {
        r.AddDoor(this);
    }

    public bool IsConnectedToRoom(Room r)
    {
        return r == RoomA || r == RoomB;
    }
}
