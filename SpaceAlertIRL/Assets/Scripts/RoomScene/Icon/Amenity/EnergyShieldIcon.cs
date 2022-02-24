using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyShieldIcon : AmenityIcon<EnergyShield>
{
    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanelSpawner>().DisplayThis(Amenity);
    }

    protected override void UpdateUI()
    {
        if (Amenity != null)
        {
            var _energyShieldValue = Amenity.ShieldValue.Value;
            var _energyShieldMaxValue = Amenity.MaxShieldValue.Value;

            GetComponentInChildren<TextMeshProUGUI>().text = $"E. Shield : {_energyShieldValue}/{_energyShieldMaxValue}";
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
