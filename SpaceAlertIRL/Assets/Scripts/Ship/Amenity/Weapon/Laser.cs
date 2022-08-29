using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Laser : Weapon<Laser>
{
    const int DamageConst = 5;
    const int RangeConst = 4;
    const float StartHeatConst = 0.0f;
    const int EnergyCostToShootConst = 1;

    public NetworkVariable<int> Damage = new NetworkVariable<int>(DamageConst);
    public NetworkVariable<int> Range = new NetworkVariable<int>(RangeConst);
    public NetworkVariable<float> Heat = new NetworkVariable<float>(StartHeatConst); // TODO: rm?
    // TODO: make NetworkVariables private


    protected override void Start()
    {
        base.Start();

        UIActions.AddOnValueChangeDependency(Damage, Range);
        UIActions.AddOnValueChangeDependency(Heat); // TODO: rm?
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShootAtClosesEnemyServerRpc(ulong clientId) //TODO: make private?
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        // request energy
        var energySource = Room.GetEnergySource();

        Enemy enemy = Zone.ComputeClosestEnemy();

        if (enemy == null)
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("noValidTargets_r", clientId: clientId); // TODO: voice track is missing
            return;
        }

        if (!energySource.PullEnergy(EnergyCostToShootConst))
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        enemy.TakeDamage(Damage.Value);
    }


    public void RequestShootingAtClosesEnemy()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtClosesEnemyServerRpc(clientId);
    }
}

