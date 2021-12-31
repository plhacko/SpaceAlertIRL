using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeIcon : Icon
{
    [SerializeField]
    private EnergyNode EnergyNode;

    public void Initialise(EnergyNode energyNode)
    {
        EnergyNode = energyNode;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        EnergyNode.UIActions.AddAction(UpdateUIAction);
    }

    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(EnergyNode);
    }

    override protected void UpdateUI()
    {
        if (EnergyNode != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {EnergyNode.GetSourceName()}";
        }
        else
        { Debug.Log("Door or NextRoom were not given to DoorIcon"); } // TODO: smazat else
    }

    override protected void OnDisable()
    {
        if (EnergyNode != null)
        {
            EnergyNode.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
