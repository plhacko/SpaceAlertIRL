#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Computer : Amenity<Computer>, IOnServerFixedUpdate
{
    const float RestartTimerConst = 160.0f; // 2m40s

    NetworkVariable<float> _Timer = new NetworkVariable<float>(RestartTimerConst);
    public float Timer { get => _Timer.Value; protected set { _Timer.Value = value; } }
    public float Timer_maxValue { get => RestartTimerConst; }
    public float GetTimeToScreensaver() => Timer;

    [SerializeField]
    GameObject Screeensaver;

    // UI
    BubbleProgressBar BubbleProgressBar;

#if SERVER
    public void ServerFixedUpdate()
    {
        float newTime = Timer - Time.deltaTime;
        if (newTime > 0.0f) { Timer = newTime; }
        else
        {
            RestartTimer();
            InstantiateScreensaverClientRpc();
        }
    }

    [ClientRpc]
    void InstantiateScreensaverClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Instantiate(Screeensaver);
    }

    public void RestartTimer(float newTimerTime = RestartTimerConst)
    {
        Timer = newTimerTime;
    }

#endif

    [ServerRpc(RequireOwnership = false)]
    public void RequestRestartTimerServerRpc()
    {
        RestartTimer();
    }

    protected override void Start()
    {
        base.Start();

        // UI
        BubbleProgressBar = GetComponent<BubbleProgressBar>();

        UIActions.AddOnValueChangeDependency(_Timer);
        UIActions.AddAction(UpdateUI);
        ServerUpdater.Add(this.gameObject);
    }

    public override void Restart()
    {
        Timer = RestartTimerConst;
    }

    void UpdateUI()
    {
        // spawn circles
        int _ = (int)(Timer / (Timer_maxValue / 6));
        BubbleProgressBar.UpdateUI(_, 5);
    }
}
