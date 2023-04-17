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
        string _status;
        if (_energyStorage > _maxEnergyStorage / 2)
            _status = "iddle";
        else if (_energyStorage > 0)
            _status = "\nless than half energy";
        else
            _status = "\nout of energy";

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = $"Status : {_status}";
        transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {_sourceName}";
        transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>().text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";
    }
}
