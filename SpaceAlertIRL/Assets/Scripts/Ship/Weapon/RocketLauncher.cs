using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RocketLauncher : Weapon<RocketLauncher>
{
    [SerializeField]
    public GameObject RocketPrefab;

    const int NumberOfRocketsConst = 4;

    public NetworkVariable<int> NumberOfRockets;

    protected override void Start()
    {
        base.Start();

        NumberOfRockets = new NetworkVariable<int>(NumberOfRocketsConst);

        UIActions.AddOnValueChangeDependency(NumberOfRockets);
    }

    
    public void RequestLaunchingRocket()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        RequestLaunchingRocketServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void RequestLaunchingRocketServerRpc(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (NumberOfRockets.Value > 0)
        {
            NumberOfRockets.Value = NumberOfRockets.Value - 1;
            Zone.GetComponentInChildren<EnemySpawner>().SpawnEnemy(RocketPrefab);
        }
        else
        {
            //TODO: make rpc
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("notEnoughRockets_r", clientId);
        }
    }
}
