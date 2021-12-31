using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyPoolActionPanel : MonoBehaviour
{
    public EnergyPool EnergyPool;
    private Action UpdateUIAction;
    // Start is called before the first frame update
    public void Initialise(EnergyPool energyPool)
    {
        EnergyPool = energyPool;
        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        EnergyPool.UIActions.AddAction(UpdateUIAction);
    }

    public void RequestEnergyTranfer()
    {
        EnergyPool.RequestEnergyTransferServerRpc();
    }

    private void UpdateUI()
    {
        var _energyStorage = EnergyPool.EnergyStorage.Value;
        var _maxEnergyStorage = EnergyPool.MaxEnergyStorage.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>().text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";
    }

    private void OnDisable()
    {
        // removes the update action
        if (EnergyPool != null)
        { EnergyPool.UIActions.RemoveAction(UpdateUIAction); }
    }
}
