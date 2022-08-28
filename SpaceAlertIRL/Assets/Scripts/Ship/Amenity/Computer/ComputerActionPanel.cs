using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerActionPanel : AmenityActionPanel<Computer>
{
    protected override void UpdateUI()
    {
        int _Timer = (int)Amenity.GetTimeToScreensaver();

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = $"screen saver in: {_Timer / 60}m {_Timer % 60}s";
    }

    public void RequestRestartTimer()
    {
        Amenity.RequestRestartTimerServerRpc();
    }
}
