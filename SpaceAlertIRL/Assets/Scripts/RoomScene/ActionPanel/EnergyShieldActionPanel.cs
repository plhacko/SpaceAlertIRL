using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;

public class EnergyShieldActionPanel : MonoBehaviour
{
    EnergyShield EnergyShield;
    private Action UpdateUIAction;

    public void Initialise(EnergyShield energyShield)
    {
        EnergyShield = energyShield;

        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        EnergyShield.UIActions.AddAction(UpdateUIAction);
    }

    public void RequestRechargeEnergyShield()
    {
        EnergyShield.RequestRechargeEnergyShieldServerRpc();
    }

    private void UpdateUI()
    {
        var _energyShieldValue = EnergyShield.ShieldValue.Value;
        var _energyShieldMaxValue = EnergyShield.MaxShieldValue.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("EnergyShield").GetComponentInChildren<TextMeshProUGUI>().text = $"E. Shield : {_energyShieldValue}/{_energyShieldMaxValue}";
    }

    private void OnDisable()
    {
        // removes the update action
        if (EnergyShield != null)
        { EnergyShield.UIActions.RemoveAction(UpdateUIAction); }
    }
}
