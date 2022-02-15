using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyPoolIcon : Icon
{
    [SerializeField]
    private EnergyPool EnergyPool;

    public void Initialise(EnergyPool energyPool)
    {
        EnergyPool = energyPool;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        EnergyPool.UIActions.AddAction(UpdateUIAction);
    }

    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(EnergyPool);
    }

    override protected void UpdateUI()
    {
        if (EnergyPool != null)
        {
            var _energyStorage = EnergyPool.EnergyStorage.Value;
            var _maxEnergyStorage = EnergyPool.MaxEnergyStorage.Value;

            GetComponentInChildren<TextMeshProUGUI>().text = $"power : {_energyStorage}/{_maxEnergyStorage}";
        }
        else
        { Debug.Log("Door or NextRoom were not given to DoorIcon"); } // TODO: smazat else
    }

    override protected void OnDisable()
    {
        if (EnergyPool != null)
        {
            EnergyPool.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
