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
    public float GetTimeToScreensaver() => Timer;

    [SerializeField]
    GameObject Screeensaver;


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

        UIActions.AddOnValueChangeDependency(_Timer);

        ServerUpdater.Add(this.gameObject);
    }

    public override void Restart()
    {
        Timer = RestartTimerConst;
    }
}
