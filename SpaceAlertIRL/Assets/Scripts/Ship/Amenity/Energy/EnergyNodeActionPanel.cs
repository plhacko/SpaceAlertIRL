using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnergyNodeActionPanel : AmenityActionPanel<EnergyNode>
{
    protected override void UpdateUI()
    {
        var _source = Amenity.GetSourceName();
        var _status = _source == null ? "not connected" : "connected";

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = $"Status : {_status}";
        transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>().text = $"Source : {_source}";
    }
}
