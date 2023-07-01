using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class EnergyShieldActionPanel : AmenityActionPanel<EnergyShield>
{
    TextMeshProUGUI Status, EnergyShield;
    Image RechargeEnergyShieldButton;

    private void Awake()
    {
        RechargeEnergyShieldButton = transform.Find("RechargeEnergyShieldButton").GetComponent<Image>();

        Status = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        EnergyShield = transform.Find("EnergyShield").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RequestRechargeEnergyShield()
    {
        ulong clientId = Unity.Netcode.NetworkManager.Singleton.LocalClientId;
        Amenity.RequestRechargeEnergyShieldServerRpc(clientId);
    }
    protected override void UpdateUI()
    {
        var _energyShieldValue = Amenity.ShieldValue;
        var _energyShieldMaxValue = Amenity.MaxShieldValue;

        Status.text = "Status : good";
        EnergyShield.text = $"E. Shield : {_energyShieldValue}/{_energyShieldMaxValue}";


        Color c = RechargeEnergyShieldButton.color;
        c.a = _energyShieldValue < _energyShieldMaxValue ? 1f : 0.6f;
        RechargeEnergyShieldButton.color = c;
    }
}
