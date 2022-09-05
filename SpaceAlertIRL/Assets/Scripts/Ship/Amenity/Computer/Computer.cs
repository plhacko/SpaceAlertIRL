#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Computer : Amenity<Computer>, IOnServerFixedUpdate
{
    const float RestartTimerConst = 120.0f; // 2min

    NetworkVariable<float> Timer = new NetworkVariable<float>(RestartTimerConst);
    public float GetTimeToScreensaver() => Timer.Value;

    [SerializeField]
    GameObject Screeensaver;


#if SERVER
    public void ServerFixedUpdate()
    {
        float newTime = Timer.Value - Time.deltaTime;
        if (newTime > 0.0f) { Timer.Value = newTime; }
        else
        {
            RestartTimer();
            InstantiateScreensaverClientRpc();
        }
    }

    [ClientRpc]
    void InstantiateScreensaverClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Instantiate(Screeensaver, parent: GameObject.Find("Canvas").transform);
    }

    public void RestartTimer(float newTimerTime = RestartTimerConst) // ulong clientId // TODO rm
    {
        Timer.Value = newTimerTime;
    }

#endif

    [ServerRpc(RequireOwnership = false)]
    public void RequestRestartTimerServerRpc() // ulong clientId // TODO rm
    {
        RestartTimer();
    }

    protected override void Start()
    {
        base.Start();

        UIActions.AddOnValueChangeDependency(Timer);

        ServerUpdater.Add(this.gameObject);
    }
}
