using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : Icon
{
    private PowerGenerator PowerGenerator;
    //rm private Action UpdateUIAction;

    public void Initialise(PowerGenerator powerGenerator)
    {
        PowerGenerator = powerGenerator;
        UpdateUIAction = UpdateUI;
        
        // old
        // PowerGenerator.MaxEnergyStorageUIActions.AddAction(UpdateUIAction);
        // PowerGenerator.EnergyPowerCellCountUIActions.AddAction(UpdateUIAction);
        // PowerGenerator.MaxEnergyStorageUIActions.AddAction(UpdateUIAction);
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
