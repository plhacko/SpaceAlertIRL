using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class RocketLauncher : Weapon<RocketLauncher>
{
    [SerializeField]
    public GameObject RocketPrefab; // must contain class Rocket

    [SerializeField]
    public ZoneNames[] TagrgetableZoneNames;

    const int NumberOfRocketsConst = 3;

    public NetworkVariable<int> NumberOfRockets;

    protected override void Start()
    {
        base.Start();

        NumberOfRockets = new NetworkVariable<int>(NumberOfRocketsConst);

        UIActions.AddOnValueChangeDependency(NumberOfRockets);
    }

    public void RequestLaunchingRocket(ZoneNames targetedzoneName)
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        RequestLaunchingRocketServerRpc(targetedzoneName, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void RequestLaunchingRocketServerRpc(ZoneNames targetedzoneName, ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (NumberOfRockets.Value > 0)
        {
            GameObject TargetedZone = GameObject.Find(targetedzoneName.ToString());

            NumberOfRockets.Value = NumberOfRockets.Value - 1;
            Enemy enemy = TargetedZone.GetComponentInChildren<EnemySpawner>().SpawnEnemy(RocketPrefab, silent: true);
            if (enemy.GetType() == typeof(Rocket))
            { ((Rocket)enemy)?.ChangeDirection(); }
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("rocketLaunched_r", removeDuplicates: false); // TODO: missing voicetrack
        }
        else
        {
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("notEnoughRockets_r", clientId: clientId);
        }
    }

    public override void Restart()
    {
        NumberOfRockets.Value = NumberOfRocketsConst;
    }
}
