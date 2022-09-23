using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : AmenityIcon<PowerGenerator>
{
    override protected void UpdateUI()
    {
        if (Amenity != null)
        {
            var _energyStorage = Amenity.EnergyStorage.Value;
            var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;
            var _energyPowerCellCount = Amenity.EnergyPowerCellCount.Value;
            GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}({_energyPowerCellCount})";
        }
    }
}
