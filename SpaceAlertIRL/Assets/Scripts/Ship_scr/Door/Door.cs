#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.UI;

public class Door : NetworkBehaviour, IRestart
{
    public static bool ShowDebugGoPanel = false;

    public const float TimeToOpenDoorsConst = 2.0f;

    public string Name { get => gameObject.name; }

    [SerializeField] public Room RoomA;
    [SerializeField] public Room RoomB;

    [SerializeField] bool IsOpenFromStart = true;
    NetworkVariable<bool> _IsOpen;
    NetworkVariable<bool> _IsLocked;
    NetworkVariable<float> _OpenningClosingProgress;

    public bool IsOpen { get => _IsOpen.Value; }
    public bool IsLocked { get => _IsLocked.Value; }
    public float OpenningClosingProgress { get => _OpenningClosingProgress.Value; }

    public UpdateUIActions UIActions = new UpdateUIActions();

    void Start()
    {
        _IsOpen = new NetworkVariable<bool>(IsOpenFromStart);
        _IsLocked = new NetworkVariable<bool>(false);
        _OpenningClosingProgress = new NetworkVariable<float>(TimeToOpenDoorsConst);


        AddSelfToRoom(RoomA);
        AddSelfToRoom(RoomB);

        UIActions.AddOnValueChangeDependency(_IsOpen);
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

        float _newOpenning = _OpenningClosingProgress.Value + deltaTime;
        if (_newOpenning <= 0)
        {
            _IsOpen.Value = false;
            _OpenningClosingProgress.Value = 0.0f;
        }
        else if (_newOpenning >= TimeToOpenDoorsConst)
        {
            _IsOpen.Value = true;
            _OpenningClosingProgress.Value = TimeToOpenDoorsConst;
        }
        else
        { _OpenningClosingProgress.Value = _newOpenning; }
    }
    [ServerRpc(RequireOwnership = false)]
    void SetIsOpenServerRpc(bool isOpen)
    {
        if (IsLocked) return;

        _IsOpen.Value = isOpen;
        _OpenningClosingProgress.Value = isOpen ? TimeToOpenDoorsConst : 0.0f;
    }

    [ServerRpc(RequireOwnership = false)]
    void LockUnlockServerRpc(bool isLocked)
    {
        _IsLocked.Value = isLocked;
        if (isLocked) // closing doors while locking
        {
            _IsOpen.Value = false;
            _OpenningClosingProgress.Value = 0.0f;
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
        _IsOpen.Value = IsOpenFromStart;
        _OpenningClosingProgress.Value = TimeToOpenDoorsConst;
    }
}
