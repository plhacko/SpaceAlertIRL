using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeIcon : AmenityIcon<EnergyNode>
{
    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanelSpawner>().DisplayThis(Amenity);
    }

    protected override void UpdateUI()
    {
        if (Amenity != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {Amenity.GetSourceName()}";
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
