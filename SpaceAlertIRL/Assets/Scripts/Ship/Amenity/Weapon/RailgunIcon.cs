using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunIcon : AmenityIcon<Railgun>
{
    BubbleProgressBar BubbleProgressBar;
    private void Awake()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();
    }

    protected override void UpdateUI()
    {

        int amountOfShots = Amenity.IsCharged ? 1 : 0;
        const int maxAmountOfShots = 1;

        // shows visually if the Railgun can be shot
        BubbleProgressBar?.UpdateUI(amountOfShots, maxAmountOfShots);

    }
}