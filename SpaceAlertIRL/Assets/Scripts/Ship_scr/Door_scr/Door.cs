#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.UI;
using System.Xml.Schema;

public class Door : NetworkBehaviour, IRestart
{
    public const float TimeToOpenDoorsConst = 2.0f;

    public string Name { get => gameObject.name; }

    [SerializeField] public Room RoomA;
    [SerializeField] public Room RoomB;

    [SerializeField] bool IsOpenFromStart = true;
    NetworkVariable<bool> _IsOpen;
    NetworkVariable<bool> _IsLocked;
    NetworkVariable<float> _OpenningClosingProgress;

    public bool IsOpen { get => _IsOpen.Value; private set { _IsOpen.Value = value; } }
    public bool IsLocked { get => _IsLocked.Value; private set { _IsLocked.Value = value; } }
    public float OpenningClosingProgress { get => _OpenningClosingProgress.Value; private set { _OpenningClosingProgress.Value = value; } }

    public UpdateUIActions UIActions = new UpdateUIActions();

    void Start()
    {
        _IsOpen = new NetworkVariable<bool>(IsOpenFromStart);
        _IsLocked = new NetworkVariable<bool>(false);
        _OpenningClosingProgress = new NetworkVariable<float>(TimeToOpenDoorsConst);


        AddSelfToRoom(RoomA);
        AddSelfToRoom(RoomB);

        UIActions.AddOnValueChangeDependency(_IsOpen, _IsLocked);
        UIActions.AddOnValueChangeDependency(_OpenningClosingProgress);

        UpdateUI();
        UIActions.AddAction(UpdateUI);
    }

    void UpdateUI()
    {
        if (IsOpen)
        { GetComponent<Image>().color = Color.white; }
        else if (IsLocked) { GetComponent<Image>().color = ProjectColors.NeonRed(); }
        else { GetComponent<Image>().color = ProjectColors.NeonYellow(); }
    }

#if (SERVER)

    [ServerRpc(RequireOwnership = false)]
    void OpenCloseServerRpc(float deltaTime, ulong clientId)
    {
        if (IsLocked)
        {
            AudioManager.Instance.RequestPlayingSentenceOnClient("doorsAreLocked_r", clientId: clientId);
            return;
        }

        float _newOpenning = OpenningClosingProgress + deltaTime;
        if (_newOpenning <= 0)
        {
            IsOpen = false;
            OpenningClosingProgress = 0.0f;
        }
        else if (_newOpenning >= TimeToOpenDoorsConst)
        {
            IsOpen = true;
            OpenningClosingProgress = TimeToOpenDoorsConst;
        }
        else
        { OpenningClosingProgress = _newOpenning; }
    }
    [ServerRpc(RequireOwnership = false)]
    void SetIsOpenServerRpc(bool isOpen)
    {
        if (IsLocked) return;

        IsOpen = isOpen;
        OpenningClosingProgress = isOpen ? TimeToOpenDoorsConst : 0.0f;
    }

    [ServerRpc(RequireOwnership = false)]
    void LockUnlockServerRpc(bool isLocked)
    {
        IsLocked = isLocked;
        if (isLocked) // closing doors while locking
        {
            IsOpen = false;
            OpenningClosingProgress = 0.0f;
        }
    }
#endif

    public void RequesOpennigFully() => SetIsOpenServerRpc(true);
    public void RequestClosingFully() => SetIsOpenServerRpc(false);

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
    public void RequestUnlocking() => LockUnlockServerRpc(false);
    public void RequestLocking() => LockUnlockServerRpc(true);

    void AddSelfToRoom(Room r) => r.AddDoor(this);

    public bool IsConnectedToRoom(Room r) => (r == RoomA || r == RoomB);
    public void Restart()
    {
        IsOpen = IsOpenFromStart;
        OpenningClosingProgress = TimeToOpenDoorsConst;
    }
}
