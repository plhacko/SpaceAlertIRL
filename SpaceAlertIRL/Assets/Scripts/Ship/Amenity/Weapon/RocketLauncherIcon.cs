using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketLauncherIcon : AmenityIcon<RocketLauncher>
{
    BubbleProgressBar BubbleProgressBar;

    private void Awake()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();
    }

    protected override void UpdateUI()
    {
        // shows visually how many rocket is being stored
        BubbleProgressBar?.UpdateUI(Amenity.NumberOfRockets, Amenity.MaxNumberOfRockets);
    }
}

