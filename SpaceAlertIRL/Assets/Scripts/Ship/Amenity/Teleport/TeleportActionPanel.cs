using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportActionPanel : AmenityActionPanel<Teleport>
{
    protected override void UpdateUI() { }

    public void TeleportCurrentPlayer()
    {
        Amenity.RequestTeleportingPlayer();
    }
}
