using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyPoolActionPanel : AmenityActionPanel<EnergyPool>
{
    public void RequestEnergyTranfer()
    {
        Amenity.RequestEnergyTransferServerRpc();
    }
    protected override void UpdateUI()
    {
        var _energyStorage = Amenity.EnergyStorage.Value;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;
        var _sourceName = Amenity.GetSourceName();

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good";
        transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {_sourceName}";
        transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>().text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";
    }
}
