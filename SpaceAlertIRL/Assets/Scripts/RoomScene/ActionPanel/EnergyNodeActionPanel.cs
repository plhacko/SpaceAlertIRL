using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeActionPanel : MonoBehaviour
{
    public EnergyNode EnergyNode;
    private Action UpdateUIAction;
    // Start is called before the first frame update
    public void Initialise(EnergyNode energyNode)
    {
        EnergyNode = energyNode;
        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        EnergyNode.UIActions.AddAction(UpdateUIAction);
    }

    private void UpdateUI()
    {
        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {EnergyNode.GetSourceName()}";
    }

    private void OnDisable()
    {
        // removes the update action
        if (EnergyNode != null)
        { EnergyNode.UIActions.RemoveAction(UpdateUIAction); }
    }
}
