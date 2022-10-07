using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// RailGun is weapon that costs no energy to fire (or at least much less energy)
// but it needs to be charged manualy
public class Railgun : Weapon<Railgun>
{
    const int DamageConst = 5;
    const float RangeConst = 75.0f;
    const int EnergyCostToShootConst = 0;

    const float TimeToChargeConst = 5.0f;
    const float TimeToDischargeConst = 5.0f;

    NetworkVariable<int> Damage = new NetworkVariable<int>(DamageConst);
    NetworkVariable<float> Range = new NetworkVariable<float>(RangeConst);
    NetworkVariable<float> ChargingTime = new NetworkVariable<float>(0.0f);

    public int GetDamageValue() => Damage.Value;
    public float GetWeaponRange() => Range.Value;
    public float GetChargingTimeValue() => ChargingTime.Value;
    public float GetTimeToChargeConst() => TimeToChargeConst;


    bool IsCharged() => ChargingTime.Value >= TimeToChargeConst;
    void Discharge() { ChargingTime.Value = 0.0f; }



    protected override void Start()
    {
        base.Start();

        UIActions.AddOnValueChangeDependency(Damage);
        UIActions.AddOnValueChangeDependency(ChargingTime, Range);
    }

    [ServerRpc(RequireOwnership = false)]
    void ChargeServerRpc(float deltaTime, ulong clientId) //must be ServerRpc
    {
        float newValue = ChargingTime.Value + deltaTime;

        if (newValue >= TimeToChargeConst)
        { ChargingTime.Value = TimeToChargeConst; }
        else
        { ChargingTime.Value = newValue; }
    }

    public void RequestCharging()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ChargeServerRpc(Time.deltaTime, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootAtClosesEnemyServerRpc(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        if (!IsCharged())
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("weaponIsNotLoaded_r", clientId: clientId);
            return;
        }

        Enemy enemy = Zone.ComputeClosestEnemy();

        if (enemy == null || enemy.Distance > GetWeaponRange())
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("noValidTargets_r", clientId: clientId);
            return;
        }

        if (!Room.EnergySource.PullEnergy(EnergyCostToShootConst))
        {
            // notify the player
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        enemy.TakeDamage(Damage.Value);
        Discharge();
    }

    public void RequestShootingAtClosesEnemy()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtClosesEnemyServerRpc(clientId);
    }

    public override void Restart()
    {
        Damage.Value = DamageConst;
        Range.Value =RangeConst;
        ChargingTime.Value = 0.0f;

    }
}
