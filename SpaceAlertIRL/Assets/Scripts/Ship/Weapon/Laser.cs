using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Laser : Weapon
{
    const int DamageConst = 5;
    const int RangeConst = 4;
    const float StartHeatConst = 0.0f;
    const int EnergyCostToShootConst = 1;

    public NetworkVariable<int> Damage;
    public NetworkVariable<int> Range;
    public NetworkVariable<float> Heat;

    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<LaserIcon>().Initialise(this);
    }


    protected override void Start()
    {
        base.Start();

        Damage = new NetworkVariable<int>(DamageConst);
        Range = new NetworkVariable<int>(RangeConst);
        Heat = new NetworkVariable<float>(StartHeatConst);

        UIActions.AddOnValueChangeDependency(Damage);
        UIActions.AddOnValueChangeDependency(Range);
        UIActions.AddOnValueChangeDependency(Heat);
    }

    public void ShootAtClosesEnemy()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        // requestenergy
        var energySource = Room.GetEnergySource();

        Enemy enemy = Zone.GetClosestEnemy();

        if (enemy == null)
        {
            //TODO: notify the player
            Debug.Log("there is no enemy to shoot at");
            return;
        }

        if (!energySource.PullEnergy(EnergyCostToShootConst))
        {
            //TODO: notify the player
            Debug.Log("there is not enough energy");
            return;
        }

        enemy.TakeDamage(Damage.Value, this);

    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestShootingAtClosesEnemyServerRpc()
    {
        ShootAtClosesEnemy();
    }
}

