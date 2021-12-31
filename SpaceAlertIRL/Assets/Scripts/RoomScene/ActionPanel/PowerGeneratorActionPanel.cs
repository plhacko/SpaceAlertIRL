using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;

public class PowerGeneratorActionPanel : MonoBehaviour
{
    public PowerGenerator PowerGenerator;
    private Action UpdateUIAction;
    // Start is called before the first frame update
    public void Initialise(PowerGenerator powerGenerator)
    {
        PowerGenerator = powerGenerator;
        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        PowerGenerator.UIActions.AddAction(UpdateUIAction);
    }


    private void UpdateUI()
    {
        var _energyStorage = PowerGenerator.EnergyStorage.Value;
        var _maxEnergyStorage = PowerGenerator.MaxEnergyStorage.Value;
        var _energyPowerCellCount = PowerGenerator.EnergyPowerCellCount.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>().text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";
        transform.Find("PowerCell").GetComponentInChildren<TextMeshProUGUI>().text = $"Power Cell : {_energyPowerCellCount}";
    }

    public void RequestBurningOfPowerCell()
    {
        PowerGenerator.RequestBurningPowerCellServerRpc();
    }

    private void OnDisable()
    {
        // removes the update action
        if (PowerGenerator != null)
        { PowerGenerator.UIActions.RemoveAction(UpdateUIAction); }
    }
}
