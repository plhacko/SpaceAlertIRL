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
    public ZoneNames[] TargetableZoneNames;

    [SerializeField]
    public ZoneNames TargetedZone;

    const int NumberOfRocketsConst = 3;

    NetworkVariable<int> _NumberOfRockets;
    public int NumberOfRockets { get => _NumberOfRockets.Value; }
    public int MaxNumberOfRockets { get => NumberOfRocketsConst; }
    
    // UI 
    BubbleProgressBar BubbleProgressBar;

    protected override void Start()
    {
        // UI
        BubbleProgressBar = GetComponent<BubbleProgressBar>();

        base.Start();

        _NumberOfRockets = new NetworkVariable<int>(NumberOfRocketsConst);

        UIActions.AddOnValueChangeDependency(_NumberOfRockets);
        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();
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

        if (_NumberOfRockets.Value > 0)
        {
            GameObject TargetedZone = GameObject.Find(targetedzoneName.ToString());

            _NumberOfRockets.Value = _NumberOfRockets.Value - 1;
            Enemy enemy = TargetedZone.GetComponentInChildren<EnemySpawner>().SpawnEnemy(RocketPrefab, silent: true);
            if (enemy.GetType() == typeof(Rocket))
            { ((Rocket)enemy)?.ChangeDirection(); }

            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
            AudioManager.Instance.RequestPlayingSentenceOnClient("rocketLaunched_r", removeDuplicates: false);
        }
        else
        {
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughRockets_r", clientId: clientId);
        }
    }

    public override void Restart()
    {
        _NumberOfRockets.Value = NumberOfRocketsConst;
    }

    void UpdateUI()
    {
        // shows visually how many rocket is being stored
        BubbleProgressBar?.UpdateUI(NumberOfRockets, MaxNumberOfRockets);
    }
}
