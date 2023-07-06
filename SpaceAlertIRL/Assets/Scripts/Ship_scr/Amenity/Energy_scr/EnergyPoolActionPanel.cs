using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnergyPoolActionPanel : AmenityActionPanel<EnergyPool>
{
    TextMeshProUGUI Status, Source, Energy;


    Image PumpEnergyButton;

    private void Awake()
    {
        Status = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Source = transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>();
        Energy = transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>();

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

        Status.text = $"Status : {_status}";
        Source.text = $"Source : {_sourceName}";
        Energy.text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";

        Color c = PumpEnergyButton.color;
        c.a = _energyStorage < _maxEnergyStorage ? 1f : 0.6f;
        PumpEnergyButton.color = c;
    }
}
