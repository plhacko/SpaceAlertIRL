using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerIcon : AmenityIcon<Computer>
{
    BubbleProgressBar BubbleProgressBar;

    void Awake()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();
    }

    protected override void UpdateUI()
    {
        if (Amenity != null)
        {
            int _ = (int)(Amenity.Timer / (Amenity.Timer_maxValue / 6));
            BubbleProgressBar.UpdateUI(_, 5);
        }
    }
}
