#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;
using System.Linq;

public class Zone : NetworkBehaviour, IRestart
{
    public UpdateUIActions UIActions = new UpdateUIActions();

    TextMeshProUGUI HPUI;
    TextMeshProUGUI ShieldUI;

    const int MaxHPConst = 5;

    private NetworkVariable<int> _HP = new NetworkVariable<int>(MaxHPConst);

    public int HP { get => _HP.Value; private set { _HP.Value = value; } }
    public int MaxHP { get => MaxHPConst; }
    public int GetShieldValue() => GetComponentsInChildren<EnergyShield>().Sum(es => es.ShieldValue);
    public int GetMaxShieldValue() => GetComponentsInChildren<EnergyShield>().Sum(es => es.MaxShieldValue);

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

        HPUI = transform.Find("HP").GetComponentInChildren<TextMeshProUGUI>();
        ShieldUI = transform.Find("Shield").GetComponentInChildren<TextMeshProUGUI>();
        UIActions.AddAction(UpdateUI);

        UIActions.UpdateUI();
    }

#if (SERVER)
    public void TakeDmage(int damage, Enemy enemy = null)
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server"); }

        // reduce damage by shiealds
        EnergyShield[] energyShieldArray = GetComponentsInChildren<EnergyShield>();
        foreach (EnergyShield es in energyShieldArray)
        {
            es.AbsorbDamage(ref damage);
        }
        if (damage == 0) { UIActions.UpdateUI(); } // updates UI if the shields dealed with all the damage

        // audio feedback
        if (damage > 0)
        { AudioManager.Instance.RequestPlayingSentenceOnClient($"{gameObject.name + "_r"} zoneDamaged_r", removeDuplicates: false); }
        else if (GetShieldValue() == 0)
        { AudioManager.Instance.RequestPlayingSentenceOnClient($"{gameObject.name + "_r"} shieldsDepleted_r", removeDuplicates: false); }


        // do damage to the ship
        int _hp = HP - damage;
        if (_hp > 0) { HP = _hp; }
        else { HP = 0; Die(); }
    }

    public void DepleteEnergyShiealds(int damage)
    {
        EnergyShield[] energyShieldArray = GetComponentsInChildren<EnergyShield>(); // in current version is just one ES, so ordering is not needed
        foreach (EnergyShield es in energyShieldArray)
        {
            es.AbsorbDamage(ref damage);
        }
        UIActions.UpdateUI();
    }

    private void Die()
    {
        ServerUpdater.StopUpdating();
        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().ChangeScene("EndScreen");
    }

    public void Restart()
    {
        HP = MaxHPConst;
    }
#endif

    void UpdateUI()
    {
        HPUI.text = HP.ToString();
        ShieldUI.text = GetShieldValue().ToString();
    }
}