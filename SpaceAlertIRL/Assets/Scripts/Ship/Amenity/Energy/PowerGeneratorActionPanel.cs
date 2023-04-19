using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

public class PowerGeneratorActionPanel : AmenityActionPanel<PowerGenerator>
{
    Image BurnPowerCellButton;

    private void Awake()
    {
        BurnPowerCellButton = transform.Find("BurnPowerCellButton").GetComponent<Image>();
    }
    protected override void UpdateUI()
    {
        var _energyStorage = Amenity.EnergyStorage;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage;
        var _energyPowerCellCount = Amenity.EnergyPowerCellCount;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good";
        transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>().text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";
        transform.Find("PowerCell").GetComponentInChildren<TextMeshProUGUI>().text = $"Power Cell : {_energyPowerCellCount}";


        Color c = BurnPowerCellButton.color;
        c.a = _energyStorage < _maxEnergyStorage ? 1f : 0.6f;
        BurnPowerCellButton.color = c;
    }

    public void RequestBurningOfPowerCell()
    {
        Amenity.RequestBurningPowerCellServerRpc();
    }
}
