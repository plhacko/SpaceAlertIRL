using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : Icon
{
    [SerializeField]
    private PowerGenerator PowerGenerator;

    public void Initialise(PowerGenerator powerGenerator)
    {
        PowerGenerator = powerGenerator;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        PowerGenerator.UIActions.AddAction(UpdateUIAction);
    }

    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(PowerGenerator);
    }

    override protected void UpdateUI()
    {
        if (PowerGenerator != null)
        {
            var _energyStorage = PowerGenerator.EnergyStorage.Value;
            var _maxEnergyStorage = PowerGenerator.MaxEnergyStorage.Value;
            var _energyPowerCellCount = PowerGenerator.EnergyPowerCellCount.Value;
            GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}({_energyPowerCellCount})";
        }
        else
        { Debug.Log("Door or NextRoom were not given to DoorIcon"); } // TODO: smazat else
    }


    override protected void OnDisable()
    {
        if (PowerGenerator != null)
        {
            PowerGenerator.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
