using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeIcon : AmenityIcon<EnergyNode>
{
    TextMeshProUGUI Source;
    private void Awake()
    {
        Source = GetComponentInChildren<TextMeshProUGUI>();
    }
    protected override void UpdateUI()
    {
        if (Amenity != null)
        {
            Source.text = $"Source : {Amenity.GetSourceName()}";
        }
    }
}
