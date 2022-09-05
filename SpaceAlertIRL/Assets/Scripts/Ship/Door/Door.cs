#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;



public class Door : NetworkBehaviour
{
    // [SerializeField]
    // float OpenningSpeedConst = 0.5f;

    public const float TimeToOpenDoorsConst = 3.0f;

    public string Name { get => gameObject.name; }

    public Room RoomA;
    public Room RoomB;


    public NetworkVariable<bool> IsOpen = new NetworkVariable<bool>(false);
    public NetworkVariable<float> OpenningClosingProgress = new NetworkVariable<float>(1.0f);

    public UpdateUIActions UIActions = new UpdateUIActions();

    void Start()
    {
        AddSelfToRoom(RoomA);
        AddSelfToRoom(RoomB);

        UIActions.AddOnValueChangeDependency(IsOpen);
        UIActions.AddOnValueChangeDependency(OpenningClosingProgress);

        // ServerUpdater.Add(this.gameObject); TODO: rm
    }


#if (SERVER)

    [ServerRpc(RequireOwnership = false)]
    void OpenCloseServerRpc(float deltaTime, ulong clientId) //must be ServerRpc
    {
        float _newOpenning = OpenningClosingProgress.Value + deltaTime;
        if (_newOpenning <= 0)
        {
            IsOpen.Value = false;
            OpenningClosingProgress.Value = 0.0f;
        }
        else if (_newOpenning >= TimeToOpenDoorsConst)
        {
            IsOpen.Value = true;
            OpenningClosingProgress.Value = TimeToOpenDoorsConst;
        }
        else
        { OpenningClosingProgress.Value = _newOpenning; }

    }
#endif

    public void RequestOpenning()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        OpenCloseServerRpc(Time.deltaTime, clientId);
    }

    public void RequestClosing()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        OpenCloseServerRpc(-Time.deltaTime, clientId);
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
