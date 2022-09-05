using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class EnergyShieldActionPanel : AmenityActionPanel<EnergyShield>
{
    public void RequestRechargeEnergyShield()
    {
        ulong clientId = Unity.Netcode.NetworkManager.Singleton.LocalClientId;
        Amenity.RequestRechargeEnergyShieldServerRpc(clientId);
    }
    protected override void UpdateUI()
    {
        var _energyShieldValue = Amenity.GetShieldValue();
        var _energyShieldMaxValue = Amenity.GetMaxShieldValue();

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("EnergyShield").GetComponentInChildren<TextMeshProUGUI>().text = $"E. Shield : {_energyShieldValue}/{_energyShieldMaxValue}";
    }
}
