using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : AmenityIcon<PowerGenerator>
{
    BubbleProgressBar BubbleProgressBar;

    void Awake()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();
    }

    override protected void UpdateUI()
    {
        if (Amenity == null)
            return;

        var _energyStorage = Amenity.EnergyStorage.Value;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;
        var _energyPowerCellCount = Amenity.EnergyPowerCellCount.Value;
        GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}({_energyPowerCellCount})";


        // spawn energy circles
        BubbleProgressBar.UpdateUI(Amenity.EnergyStorage.Value, Amenity.MaxEnergyStorage.Value);

    }
}
