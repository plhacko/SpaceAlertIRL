#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Zone : NetworkBehaviour, IRestart
{
    public UpdateUIActions UIActions = new UpdateUIActions();

    const int MaxHPConst = 5;

    NetworkVariable<int> _HP = new NetworkVariable<int>(MaxHPConst);

    public int HP { get => _HP.Value; }
    public int MaxHP { get => MaxHPConst; }

    public Enemy[] GenrateSortedEnemyArray()
    {
        Enemy[] enemiesInZone = GetComponentsInChildren<Enemy>();
        Array.Sort(enemiesInZone);

        return enemiesInZone;
    }

    public Enemy ComputeClosestEnemy()
    {
        Enemy[] enemiesInZone = GetComponentsInChildren<Enemy>();
        if (enemiesInZone.Length == 0) { return null; }

        Enemy closestEnemy = enemiesInZone[0];
        foreach (var e in enemiesInZone)
        {
            if (closestEnemy.Distance > e.Distance)
            { closestEnemy = e; }
        }
        return closestEnemy;
    }

    private void Start()
    {
        UIActions.AddOnValueChangeDependency(_HP);
    }

#if (SERVER)
    public void TakeDmage(int damage, Enemy enemy = null)
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server"); }

        // reduce damage by shiealds
        EnergyShield[] energyShieldArray = GetComponentsInChildren<EnergyShield>(); // TODO: ? array sort this based on smothing ?
        foreach (EnergyShield es in energyShieldArray)
        {
            es.AbsorbDamage(ref damage);
        }

        // do damage to the ship
        int _hp = _HP.Value - damage;
        if (_hp > 0) { _HP.Value = _hp; }
        else { _HP.Value = 0; Die(); }
    }

    public void DepleteEnergyShiealds(int damage)
    {
        EnergyShield[] energyShieldArray = GetComponentsInChildren<EnergyShield>(); // TODO: ? array sort this based on smothing ?
        foreach (EnergyShield es in energyShieldArray)
        {
            es.AbsorbDamage(ref damage);
        }
    }

    private void Die()
    {
        ServerUpdater.StopUpdating();
        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().ChangeScene("EndScreen");
    }

    public void Restart()
    {
        _HP.Value = MaxHPConst;
    }
#endif

}
