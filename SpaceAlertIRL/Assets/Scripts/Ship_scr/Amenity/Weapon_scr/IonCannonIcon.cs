using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonCannonIcon : AmenityIcon<IonCannon>
{
    BubbleProgressBar BubbleProgressBar;

    private void Awake()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();
    }

    protected override void UpdateUI()
    {
        int amountOfShots = Amenity.IsTooHotToShoot ? 0 : 1;
        const int maxAmountOfShots = 1;

        // shows visually if the IonCannon can be shot
        BubbleProgressBar?.UpdateUI(amountOfShots, maxAmountOfShots);
    }
}
