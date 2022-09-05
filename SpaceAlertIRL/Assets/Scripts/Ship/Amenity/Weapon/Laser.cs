using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Laser : Weapon<Laser>, IOnServerFixedUpdate
{
    const int DamageConst = 5;
    const int RangeConst = 4;
    const int EnergyCostToShootConst = 1;
    const float StartHeatConst = 0.0f; // 0%
    const float MaxHeatConst = 100.0f; // 0%
    const float HeatReductionConst = 5.0f; // 5% per second
    const float HeatCostPerShotConst = 50.0f;

    NetworkVariable<int> Damage = new NetworkVariable<int>(DamageConst);
    NetworkVariable<int> Range = new NetworkVariable<int>(RangeConst);
    NetworkVariable<float> Heat = new NetworkVariable<float>(StartHeatConst);

    public int GetWeaponDamage() => Damage.Value;
    public int GetWeaponRange() => Range.Value;
    public float GetWeaponHeat() => Heat.Value;

    public bool IsTooHotToShoot() => Heat.Value + HeatCostPerShotConst > MaxHeatConst;

    protected override void Start()
    {
        base.Start();

        UIActions.AddOnValueChangeDependency(Damage, Range);
        UIActions.AddOnValueChangeDependency(Heat);

        ServerUpdater.Add(this);
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootAtClosesEnemyServerRpc(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        // heat check
        if (IsTooHotToShoot())
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("highHeatAlert_r", clientId: clientId); // TODO: voice track is missing
            return;
        }

        // enemy in range check
        Enemy enemy = Zone.ComputeClosestEnemy();
        if (enemy == null)
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("noValidTargets_r", clientId: clientId); // TODO: voice track is missing
            return;
        }

        // energy check
        if (!Room.EnergySource.PullEnergy(EnergyCostToShootConst))
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        Heat.Value = Heat.Value + HeatCostPerShotConst;
        enemy.TakeDamage(Damage.Value);
    }


    public void RequestShootingAtClosesEnemy()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtClosesEnemyServerRpc(clientId);
    }

    public void ServerFixedUpdate()
    {
        float newHeat = Heat.Value -= HeatReductionConst * Time.deltaTime;

        if (newHeat <= 0.0f)
        { Heat.Value = 0.0f; }
        else
        { Heat.Value = newHeat; }
    }
}

