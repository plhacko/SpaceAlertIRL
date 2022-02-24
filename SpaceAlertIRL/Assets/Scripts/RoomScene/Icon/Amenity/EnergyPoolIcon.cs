using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyPoolIcon : AmenityIcon<EnergyPool>
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

            GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}";
        }
        else
        { Debug.Log("Door or NextRoom were not given to DoorIcon"); } // TODO: smazat else
    }
}
