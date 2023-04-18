using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyShieldIcon : AmenityIcon<EnergyShield>
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
            var _energyShieldValue = Amenity.ShieldValue;
            var _energyShieldMaxValue = Amenity.MaxShieldValue;

            GetComponentInChildren<TextMeshProUGUI>().text = $"E. Shield : {_energyShieldValue}/{_energyShieldMaxValue}";

            // spawn energy circles
            BubbleProgressBar.UpdateUI(Amenity.ShieldValue, Amenity.MaxShieldValue);
        }
    }
}
