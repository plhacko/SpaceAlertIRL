using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// RailGun is weapon that costs no energy to fire (or at least much less energy - to be decided (TODO:))
// but it needs to be charged manualy
public class Railgun : Weapon<Railgun>
{
    const int DamageConst = 5;
    const int RangeConst = 4;
    const int EnergyCostToShootConst = 0;

    const float TimeToChargeConst = 5.0f;
    const float TimeToDischargeConst = 5.0f;

    NetworkVariable<int> Damage = new NetworkVariable<int>(DamageConst);
    NetworkVariable<int> Range = new NetworkVariable<int>(RangeConst);
    NetworkVariable<float> ChargingTime = new NetworkVariable<float>(0.0f);

    public int GetDamageValue() => Damage.Value;
    public int GetRangeValue() => Range.Value;
    public float GetChargingTimeValue() => ChargingTime.Value;
    public float GetTimeToChargeConst() => TimeToChargeConst;

    bool IsCharged() => ChargingTime.Value >= TimeToChargeConst;
    void Discharge() { ChargingTime.Value = 0.0f; }



    protected override void Start()
    {
        base.Start();

        UIActions.AddOnValueChangeDependency(Damage, Range);
        UIActions.AddOnValueChangeDependency(ChargingTime);
    }

    [ServerRpc(RequireOwnership = false)]
    void ChargeServerRpc(float deltaTime, ulong clientId) //must be ServerRpc
    {
        float newValue = ChargingTime.Value + deltaTime;

        if (newValue >= TimeToChargeConst)
        {
            ChargingTime.Value = TimeToChargeConst;
        }
        else
        {
            ChargingTime.Value = newValue;
        }
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
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("weaponIsNotLoaded_r", clientId: clientId); // TODO: voice track is missing        
            return;
        }

        // get energy source
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
        Discharge();
    }

    public void RequestShootingAtClosesEnemy()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtClosesEnemyServerRpc(clientId);
    }
}
