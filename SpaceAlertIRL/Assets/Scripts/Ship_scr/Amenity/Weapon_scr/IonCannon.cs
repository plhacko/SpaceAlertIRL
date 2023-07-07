using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class IonCannon : Weapon<IonCannon>, IOnServerFixedUpdate
{
    [SerializeField] public ZoneNames[] TargetableZoneNames;
    [SerializeField] public ZoneNames TargetedZone;

    const int DamageConst = 5;
    const RangeEnum RangeConst = RangeEnum.Close;
    const int EnergyCostToShootConst = 2;
    const float StartHeatConst = 0.0f; // 0%
    const float MaxHeatConst = 100.0f; // 100%
    const float HeatCostPerShotConst = MaxHeatConst;
    const float NormalCoolingModifierConst = 3.0f;

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
    void ShootAtEnemiesServerRpc(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        // heat check
        if (IsTooHotToShoot)
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("highHeatAlert_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.error, clientId: clientId);
            return;
        }

        // get enemies in range
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach (Enemy e in Zone.transform.parent.GetComponentsInChildren<Enemy>())
        {
            if (e.Distance < Range)
                enemiesInRange.Add(e);
        }

        // no enemy in range check
        if (enemiesInRange.Count == 0)
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("noValidTargets_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.error, clientId: clientId);
            return;
        }

        // energy check
        if (!Room.EnergySource.PullEnergy(EnergyCostToShootConst))
        {
            // notify the player
            AudioManager.Instance.RequestPlayingSentenceOnClient("notEnoughEnergy_r", clientId: clientId);
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.error, clientId: clientId);
            return;
        }

        // success
        AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
        AudioManager.Instance.RequestPlayingSentenceOnClient("enemyDamaged_r", clientId: clientId); // TODO: add voicetrack

        Heat += HeatCostPerShotConst;
        foreach (Enemy e in enemiesInRange)
        {
            e.DeleteEnergyShields(Damage);
        }
    }


    public void RequestShootingAtEnemies()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ShootAtEnemiesServerRpc(clientId);
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

