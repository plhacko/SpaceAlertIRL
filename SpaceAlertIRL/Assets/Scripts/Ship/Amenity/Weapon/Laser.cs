using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Laser : Weapon<Laser>, IOnServerFixedUpdate
{
    const int DamageConst = 4;
    const RangeEnum RangeConst = RangeEnum.Far;
    const int EnergyCostToShootConst = 1;
    const int EnergyCostToActiveCoolingConst = 1;
    const float StartHeatConst = 0.0f; // 0%
    const float MaxHeatConst = 100.0f; // 100%
    const float HeatCostPerShotConst = MaxHeatConst;
    const float NormalCoolingModifierConst = 3.0f;
    const float ActiveCoolingModifierConst = 2.0f * NormalCoolingModifierConst;

    NetworkVariable<int> Damage = new NetworkVariable<int>(DamageConst);
    NetworkVariable<float> Range = new NetworkVariable<float>((float)RangeConst);
    NetworkVariable<float> Heat = new NetworkVariable<float>(StartHeatConst);
    NetworkVariable<float> CoolingModifier = new NetworkVariable<float>(NormalCoolingModifierConst);

    public int GetWeaponDamage() => Damage.Value;
    public float GetWeaponRange() => Range.Value;
    public float GetWeaponHeat() => Heat.Value;

    public bool IsTooHotToShoot() => Heat.Value > 0;
    public bool IsActivelyCooled() => CoolingModifier.Value == ActiveCoolingModifierConst;

    protected override void Start()
    {
        base.Start();

        UIActions.AddOnValueChangeDependency(Damage);
        UIActions.AddOnValueChangeDependency(Heat, Range, CoolingModifier);

        ServerUpdater.Add(this.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootAtClosestEnemyServerRpc(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        // heat check
        if (IsTooHotToShoot())
        {
            // notify the player
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("highHeatAlert_r", clientId: clientId);
            return;
        }

        // no enemy in range check
        Enemy enemy = Zone.ComputeClosestEnemy();
        if (enemy == null || enemy.Distance > GetWeaponRange())
        {
            // notify the player
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("noValidTargets_r", clientId: clientId);
            return;
        }

        // energy check
        if (!Room.EnergySource.PullEnergy(EnergyCostToShootConst))
        {
            // notify the player
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        Heat.Value = Heat.Value + HeatCostPerShotConst;
        enemy.TakeDamage(Damage.Value);
    }


    public void RequestShootingAtClosestEnemy()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtClosestEnemyServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void ActivateActiveCoolingServerRpc(ulong clientId)
    {
        // cooling already active check
        if (IsActivelyCooled())
        {
            // notify the player
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("CoolingAlreadyActive_r", clientId: clientId); // TODO: add voice track
            return;
        }

        // already cool check
        if (!IsTooHotToShoot())
        {
            // notify the player
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("NoAditionalCoolingNeeded_r", clientId: clientId); // TODO: add voice track
            return;
        }

        // energy check
        if (!Room.EnergySource.PullEnergy(EnergyCostToActiveCoolingConst))
        {
            // notify the player
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        CoolingModifier.Value = ActiveCoolingModifierConst;
    }


    public void RequestActivateActiveCooling()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ActivateActiveCoolingServerRpc(clientId);
    }


    public void ServerFixedUpdate()
    {
        float newHeat = Heat.Value - Time.deltaTime * CoolingModifier.Value;

        if (newHeat <= 0.0f)
        {
            Heat.Value = 0.0f;
            CoolingModifier.Value = NormalCoolingModifierConst;
        }
        else
        { Heat.Value = newHeat; }
    }

    public override void Restart()
    {
        Damage.Value = DamageConst;
        Range.Value = (float)RangeConst;
        Heat.Value = StartHeatConst;
    }
}

