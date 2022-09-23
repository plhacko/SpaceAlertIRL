using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeIcon : AmenityIcon<EnergyNode>
{
    protected override void UpdateUI()
    {
        if (Amenity != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {Amenity.GetSourceName()}";
        }
    }
}
