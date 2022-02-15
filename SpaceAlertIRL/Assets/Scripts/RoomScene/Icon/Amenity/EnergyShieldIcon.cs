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
            var _energyShieldValue = EnergyShield.ShieldValue.Value;
            var _energyShieldMaxValue = EnergyShield.MaxShieldValue.Value;

            GetComponentInChildren<TextMeshProUGUI>().text = $"E. Shield : {_energyShieldValue}/{_energyShieldMaxValue}";
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }

    public void Initialise(EnergyShield energyShield)
    {
        EnergyShield = energyShield;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        EnergyShield.UIActions.AddAction(UpdateUIAction);
    }
}
