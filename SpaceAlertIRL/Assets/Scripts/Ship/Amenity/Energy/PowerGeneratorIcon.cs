using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : AmenityIcon<PowerGenerator>
{
    [SerializeField] GameObject EnergyCircle_full_prefab;
    [SerializeField] GameObject EnergyCircle_empty_prefab;

    override protected void UpdateUI()
    {
        if (Amenity == null)
            return;

        var _energyStorage = Amenity.EnergyStorage.Value;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;
        var _energyPowerCellCount = Amenity.EnergyPowerCellCount.Value;
        GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}({_energyPowerCellCount})";


        // spawn energy circles
        // reset self
        foreach (Transform child in transform)
        {
            if (child.name != "Text")
                GameObject.Destroy(child.gameObject);
        }
        // spawn energy circles
        for (int i = 0; i < Amenity.EnergyStorage.Value; i++)
        {
            Instantiate(EnergyCircle_full_prefab, parent: transform);
        }
        for (int j = Amenity.EnergyStorage.Value; j < Amenity.MaxEnergyStorage.Value; j++)
        {
            Instantiate(EnergyCircle_empty_prefab, parent: transform);
        }
    }
}
