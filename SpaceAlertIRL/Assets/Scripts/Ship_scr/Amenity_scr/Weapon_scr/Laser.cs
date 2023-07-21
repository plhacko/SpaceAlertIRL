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
    const float NormalCoolingModifierConst = 2.0f;
    const float ActiveCoolingModifierConst = 2.0f * NormalCoolingModifierConst;
    // note: Active cooling functionality is implemented, but was finally decided not to use it

    NetworkVariable<int> _Damage = new NetworkVariable<int>(DamageConst);
    NetworkVariable<float> _Range = new NetworkVariable<float>((float)RangeConst);
    NetworkVariable<float> _Heat = new NetworkVariable<float>(StartHeatConst);
    NetworkVariable<float> _CoolingModifier = new NetworkVariable<float>(NormalCoolingModifierConst);

    public int Damage { get => _Damage.Value; private set { _Damage.Value = value; } }
    public float Range { get => _Range.Value; private set { _Range.Value = value; } }
    public float Heat { get => _Heat.Value; private set { _Heat.Value = value; } }
    public float CoolingModifier { get => _CoolingModifier.Value; private set { _CoolingModifier.Value = value; } }
    public int EnergyCost { get => EnergyCostToShootConst; }

    public bool IsTooHotToShoot { get => Heat > 0; }
    public bool IsActivelyCooled { get => CoolingModifier == ActiveCoolingModifierConst; }

    // UI 
    BubbleProgressBar BubbleProgressBar;

    protected override void Start()
    {
        // UI
        BubbleProgressBar = GetComponent<BubbleProgressBar>();

        base.Start();

        UIActions.AddOnValueChangeDependency(_Damage);
        UIActions.AddOnValueChangeDependency(_Heat, _Range, _CoolingModifier);
        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();

        ServerUpdater.Add(this.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootAtClosestEnemyServerRpc(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        // heat check
        if (IsTooHotToShoot)
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("highHeatAlert_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
            return;
        }

        // no enemy in range check
        Enemy enemy = Zone.ComputeClosestEnemy();
        if (enemy == null || enemy.Distance > Range)
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("noValidTargets_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
            return;
        }

        // energy check
        if (!Room.EnergySource.PullEnergy(EnergyCostToShootConst))
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
            return;
        }

        // success
        Heat += HeatCostPerShotConst;
        enemy.TakeDamage(Damage);
        
        AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
    }


    public void RequestShootingAtClosestEnemy()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtClosestEnemyServerRpc(clientId);
    }

    // is not currently used in the game, code might be not up to date
    [ServerRpc(RequireOwnership = false)]
    void ActivateActiveCoolingServerRpc(ulong clientId)
    {
        // cooling already active check
        if (IsActivelyCooled)
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("coolingAlreadyActive_r", clientId: clientId);
            return;
        }

        // already cool check
        if (!IsTooHotToShoot)
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("noAditionalCoolingNeeded_r", clientId: clientId);
            return;
        }

        // energy check
        if (!Room.EnergySource.PullEnergy(EnergyCostToActiveCoolingConst))
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            return;
        }

        CoolingModifier = ActiveCoolingModifierConst;
    }

    // is not currently used in the game, code might be not up to date
    public void RequestActivateActiveCooling()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ActivateActiveCoolingServerRpc(clientId);
    }


    public void ServerFixedUpdate()
    {
        float newHeat = Heat - Time.deltaTime * CoolingModifier;

        if (newHeat <= 0.0f)
        {
            Heat = 0.0f;
            CoolingModifier = NormalCoolingModifierConst;
        }
        else
        { Heat = newHeat; }
    }

    public override void Restart()
    {
        Damage = DamageConst;
        Range = (float)RangeConst;
        Heat = StartHeatConst;
    }

    void UpdateUI()
    {
        int amountOfShots = IsTooHotToShoot ? 0 : 1;
        const int maxAmountOfShots = 1;

        // shows visually if the Laser can be shot
        BubbleProgressBar?.UpdateUI(amountOfShots, maxAmountOfShots);
    }
}

