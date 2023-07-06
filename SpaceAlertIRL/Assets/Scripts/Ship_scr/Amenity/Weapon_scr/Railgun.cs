using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// RailGun is weapon that costs no energy to fire (or at least much less energy)
// but it needs to be charged manualy
public class Railgun : Weapon<Railgun>
{
    const int DamageConst = 3;
    const RangeEnum RangeConst = RangeEnum.Mid;
    const int EnergyCostToShootConst = 0;

    const float TimeToChargeConst = 5.0f;
    const float TimeToDischargeConst = 5.0f;

    NetworkVariable<int> _Damage = new NetworkVariable<int>(DamageConst);
    NetworkVariable<float> _Range = new NetworkVariable<float>((float)RangeConst);
    NetworkVariable<float> _ChargingTime = new NetworkVariable<float>(0.0f);

    // UI 
    BubbleProgressBar BubbleProgressBar;

    public int Damage { get => _Damage.Value; private set { _Damage.Value = value; } }
    public float Range { get => _Range.Value; private set { _Range.Value = value; } }
    public float ChargingTime { get => _ChargingTime.Value; private set { _ChargingTime.Value = value; } }
    public float TimeToCharge { get => TimeToChargeConst; }


    public bool IsCharged { get => ChargingTime >= TimeToChargeConst; }
    void Discharge() { ChargingTime = 0.0f; }



    protected override void Start()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();

        base.Start();

        UIActions.AddOnValueChangeDependency(_Damage);
        UIActions.AddOnValueChangeDependency(_ChargingTime, _Range);
        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();
    }

    [ServerRpc(RequireOwnership = false)]
    void ChargeServerRpc(float deltaTime, ulong clientId) //must be ServerRpc
    {
        float newValue = ChargingTime + deltaTime;

        if (newValue >= TimeToChargeConst)
        {
            ChargingTime = TimeToChargeConst;
            AudioManager.Instance.RequestPlayingSentenceOnClient("railGunCharged_r", clientId: clientId); // TODO: missing
        }
        else
        {
            ChargingTime = newValue;
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
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

        if (!IsCharged)
        {
            AudioManager.Instance.RequestPlayingSentenceOnClient("weaponIsNotLoaded_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.error, clientId: clientId);
            return;
        }

        Enemy enemy = Zone.ComputeClosestEnemy();

        if (enemy == null || enemy.Distance > Range)
        {
            // notify the player
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.error, clientId: clientId);
            AudioManager.Instance.RequestPlayingSentenceOnClient("noValidTargets_r", clientId: clientId);
            return;
        }

        if (!Room.EnergySource.PullEnergy(EnergyCostToShootConst)) // curenty the cost is 0, in future it might change
        {
            // notify the player
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.error, clientId: clientId);
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        enemy.TakeDamage(Damage);
        Discharge();

        AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
        if (enemy != null)
            AudioManager.Instance.RequestPlayingSentenceOnClient("enemyDamaged_r", clientId: clientId);
    }

    public void RequestShootingAtClosesEnemy()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtClosesEnemyServerRpc(clientId);
    }

    public override void Restart()
    {
        Damage = DamageConst;
        Range = (float)RangeConst;
        ChargingTime = 0.0f;

    }

    void UpdateUI()
    {
        int amountOfShots = IsCharged ? 1 : 0;
        const int maxAmountOfShots = 1;

        // shows visually if the Railgun can be shot
        BubbleProgressBar?.UpdateUI(amountOfShots, maxAmountOfShots);
    }
}
