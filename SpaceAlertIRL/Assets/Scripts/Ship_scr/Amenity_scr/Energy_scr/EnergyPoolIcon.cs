using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyPoolIcon : AmenityIcon<EnergyPool>
{
    BubbleProgressBar BubbleProgressBar;

    void Awake()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();
    }

    override protected void UpdateUI()
    {
        if (Amenity == null) return;

        var _energyStorage = Amenity.EnergyStorage;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage;

        // spawn energy circles
        BubbleProgressBar.UpdateUI(_energyStorage, _maxEnergyStorage);
    }
}
