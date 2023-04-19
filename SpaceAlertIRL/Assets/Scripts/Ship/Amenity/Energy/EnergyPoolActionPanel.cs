using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class EnergyPoolActionPanel : AmenityActionPanel<EnergyPool>
{
    Image PumpEnergyButton;

    private void Awake()
    {
        PumpEnergyButton = transform.Find("PumpEnergyButton").GetComponent<Image>();
    }

    public void RequestEnergyTranfer()
    {
        Amenity.RequestEnergyTransferServerRpc();
    }
    protected override void UpdateUI()
    {
        var _energyStorage = Amenity.EnergyStorage;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage;
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

        
        Color c = PumpEnergyButton.color;
        c.a = _energyStorage < _maxEnergyStorage ? 1f : 0.6f;
        PumpEnergyButton.color = c;
    }
}
