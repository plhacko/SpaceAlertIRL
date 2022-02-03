using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyShieldIcon : Icon
{
    [SerializeField]
    private EnergyShield EnergyShield;

    protected override void OnDisable()
    {
        if (EnergyShield != null)
        {
            EnergyShield.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(EnergyShield);
    }

    protected override void UpdateUI()
    {
        if (EnergyShield != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"E. Shield : {3}/{5}";
        }
        else
        { Debug.Log("Door or NextRoom were not given to DoorIcon"); } // TODO: smazat else
    }

    public void Initialise(EnergyShield energyShield)
    {
        EnergyShield = energyShield;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        EnergyShield.UIActions.AddAction(UpdateUIAction);
    }
}
