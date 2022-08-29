using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleportActionPanel : AmenityActionPanel<Teleport>
{
    protected override void UpdateUI()
    {
        var _energyCost = Amenity.GetEnergyCost();

        transform.Find("EnergyCost").GetComponentInChildren<TextMeshProUGUI>().text = $"energy cost : {_energyCost}";
    }

    public void TeleportCurrentPlayer()
    {
        Amenity.RequestTeleportingPlayer();
    }
}
