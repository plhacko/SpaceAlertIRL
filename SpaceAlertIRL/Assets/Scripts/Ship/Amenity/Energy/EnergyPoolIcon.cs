using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyPoolIcon : AmenityIcon<EnergyPool>
{
    override protected void UpdateUI()
    {
        if (Amenity != null)
        {
            var _energyStorage = Amenity.EnergyStorage.Value;
            var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;

            GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}";
        }
    }
}
