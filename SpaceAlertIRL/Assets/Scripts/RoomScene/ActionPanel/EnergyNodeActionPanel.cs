using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeActionPanel : AmenityActionPanel<EnergyNode>
{
    protected override void UpdateUI()
    {
        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {Amenity.GetSourceName()}";
    }
}
