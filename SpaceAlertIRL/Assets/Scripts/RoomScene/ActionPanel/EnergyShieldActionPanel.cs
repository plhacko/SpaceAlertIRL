using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;

public class EnergyShieldActionPanel : ActionPanel<EnergyShield>
{
    public void RequestRechargeEnergyShield()
    {
        Amenity.RequestRechargeEnergyShieldServerRpc();
    }
    protected override void UpdateUI()
    {
        var _energyShieldValue = Amenity.ShieldValue.Value;
        var _energyShieldMaxValue = Amenity.MaxShieldValue.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("EnergyShield").GetComponentInChildren<TextMeshProUGUI>().text = $"E. Shield : {_energyShieldValue}/{_energyShieldMaxValue}";
    }
}
