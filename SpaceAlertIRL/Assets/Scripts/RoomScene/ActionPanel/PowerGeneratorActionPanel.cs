using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;

public class PowerGeneratorActionPanel : ActionPanel<PowerGenerator>
{
    protected override void UpdateUI()
    {
        var _energyStorage = Amenity.EnergyStorage.Value;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;
        var _energyPowerCellCount = Amenity.EnergyPowerCellCount.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>().text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";
        transform.Find("PowerCell").GetComponentInChildren<TextMeshProUGUI>().text = $"Power Cell : {_energyPowerCellCount}";
    }

    public void RequestBurningOfPowerCell()
    {
        Amenity.RequestBurningPowerCellServerRpc();
    }
}
