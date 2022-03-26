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

    
    public void LaunchRocket()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (NumberOfRockets.Value > 0)
        {
            NumberOfRockets.Value = NumberOfRockets.Value - 1;
            Zone.GetComponentInChildren<EnemySpawner>().SpawnEnemy(RocketPrefab);
        }
        else
        { Debug.Log("Number of rocket is zero"); } //TODO: add noice notification
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestLaunchingRocketServerRpc()
    {
        LaunchRocket();
    }
}