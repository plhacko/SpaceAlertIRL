using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleportActionPanel : AmenityActionPanel<Teleport>
{
    TextMeshProUGUI EnergyCost;

    private void Awake()
    {
        EnergyCost = transform.Find("EnergyCost").GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void UpdateUI()
    {
        var _energyCost = Amenity.GetEnergyCost();

        EnergyCost.text = $"energy cost : {_energyCost}";
    }

    public void TeleportCurrentPlayer()
    {
        Amenity.RequestTeleportingPlayer();
    }
}
