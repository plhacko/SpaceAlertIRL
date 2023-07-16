using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerActionPanel : AmenityActionPanel<Computer>
{
    TextMeshProUGUI Status;
    private void Awake()
    {
        Status = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void UpdateUI()
    {
        int _Timer = (int)Amenity.GetTimeToScreensaver();

        Status.text = $"screen saver in: {_Timer / 60}m {_Timer % 60}s";
    }

    public void RequestRestartTimer()
    {
        ulong clientId = Unity.Netcode.NetworkManager.Singleton.LocalClientId;
        Amenity.RequestRestartTimerServerRpc(clientId: clientId);
    }
}
