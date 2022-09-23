#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.UI;

public class Door : NetworkBehaviour
{
    public const float TimeToOpenDoorsConst = 2.0f;

    public string Name { get => gameObject.name; }

    public Room RoomA;
    public Room RoomB;

    public NetworkVariable<bool> IsOpen = new NetworkVariable<bool>(false);
    public NetworkVariable<float> OpenningClosingProgress = new NetworkVariable<float>(0.0f);

    public UpdateUIActions UIActions = new UpdateUIActions();

    void Start()
    {
        AddSelfToRoom(RoomA);
        AddSelfToRoom(RoomB);

        UIActions.AddOnValueChangeDependency(IsOpen);
        UIActions.AddOnValueChangeDependency(OpenningClosingProgress);

        UpdateUI();
        UIActions.AddAction(UpdateUI);
    }

    void UpdateUI()
    {
        if (IsOpen.Value)
        { GetComponent<Image>().color = Color.green; }
        else { GetComponent<Image>().color = Color.white; }
    }

#if (SERVER)

    [ServerRpc(RequireOwnership = false)]
    void OpenCloseServerRpc(float deltaTime, ulong clientId)
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
    [ServerRpc(RequireOwnership = false)]
    void SetIsOpenServerRpc(bool isOpen)
    {
        IsOpen.Value = isOpen;
        OpenningClosingProgress.Value = isOpen ? TimeToOpenDoorsConst : 0.0f;
    }
#endif

    public void RequesOpennigFully() { SetIsOpenServerRpc(true); }
    public void RequestClosingFully() { SetIsOpenServerRpc(false); }

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
