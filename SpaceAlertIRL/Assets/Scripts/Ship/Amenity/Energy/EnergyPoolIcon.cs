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

        var _energyStorage = Amenity.EnergyStorage.Value;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;

        GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}";

        // spawn energy circles
        BubbleProgressBar.UpdateUI(Amenity.EnergyStorage.Value, Amenity.MaxEnergyStorage.Value);
    }
}
