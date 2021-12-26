using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : MonoBehaviour
{
    private PowerGenerator PowerGenerator;
    private Action UpdateUIAction;

    public void Initialise(PowerGenerator powerGenerator)
    {
        PowerGenerator = powerGenerator;
        UpdateUIAction = UpdateUI;

        // PowerGenerator.MaxEnergyStorageUIActions.AddAction(UpdateUIAction);
        // PowerGenerator.EnergyPowerCellCountUIActions.AddAction(UpdateUIAction);
        // PowerGenerator.MaxEnergyStorageUIActions.AddAction(UpdateUIAction);
    }

    private void UpdateUI()
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

    private void OnDisable()
    {
        if (PowerGenerator != null)
        {
            // PowerGenerator.MaxEnergyStorageUIActions.RemoveAction(UpdateUIAction);
            // PowerGenerator.EnergyPowerCellCountUIActions.RemoveAction(UpdateUIAction);
            // PowerGenerator.MaxEnergyStorageUIActions.RemoveAction(UpdateUIAction);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
