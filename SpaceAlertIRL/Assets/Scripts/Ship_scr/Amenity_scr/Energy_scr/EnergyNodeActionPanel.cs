using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeActionPanel : AmenityActionPanel<EnergyNode>
{
    TextMeshProUGUI Status, Source;
    private void Awake()
    {
        Status = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Source = transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>();
    }
    protected override void UpdateUI()
    {
        var _source = Amenity.GetSourceName();
        var _status = _source == null ? "not connected" : "connected";

        Status.text = $"Status : {_status}";
        Source.text = $"Source : {_source}";
    }
}
