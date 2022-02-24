using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : AmenityIcon<PowerGenerator>
{
    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanelSpawner>().DisplayThis(Amenity);
    }

    override protected void UpdateUI()
    {
        if (Amenity != null)
        {
            var _energyStorage = Amenity.EnergyStorage.Value;
            var _maxEnergyStorage = Amenity.MaxEnergyStorage.Value;
            var _energyPowerCellCount = Amenity.EnergyPowerCellCount.Value;
            GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}({_energyPowerCellCount})";
        }
        else
        { Debug.Log("Door or NextRoom were not given to DoorIcon"); } // TODO: smazat else
    }
}
