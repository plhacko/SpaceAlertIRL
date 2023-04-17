using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyPoolIcon : AmenityIcon<EnergyPool>
{
    [SerializeField] GameObject EnergyCircle_full_prefab;
    [SerializeField] GameObject EnergyCircle_empty_prefab;

    override protected void UpdateUI()
    {
        if (Amenity == null) return;

        var _energyStorage = Amenity.EnergyStorage.Value;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;

        GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}";


        // spawn energy circles
        // reset self
        foreach (Transform child in transform)
        {
            if (child.name != "Text")
                GameObject.Destroy(child.gameObject);
        }
        // spawn energy circles
        for (int i = 0; i < _energyStorage; i++)
        {
            Instantiate(EnergyCircle_full_prefab, parent: transform);
        }
        for (int j = _energyStorage; j < _maxEnergyStorage; j++)
        {
            Instantiate(EnergyCircle_empty_prefab, parent: transform);
        }
    }
}
