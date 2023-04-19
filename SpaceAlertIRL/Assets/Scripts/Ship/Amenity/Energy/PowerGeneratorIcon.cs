using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PowerGeneratorIcon : AmenityIcon<PowerGenerator>
{
    BubbleProgressBar BubbleProgressBar;

    void Awake()
    {
        BubbleProgressBar = GetComponent<BubbleProgressBar>();
    }

    override protected void UpdateUI()
    {
        if (Amenity == null)
            return;

        // spawn energy circles
        BubbleProgressBar.UpdateUI(Amenity.EnergyStorage, Amenity.MaxEnergyStorage);

    }
}
