using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserIcon : AmenityIcon<Laser>
{
    protected override void UpdateUI()
    {
        if (Amenity != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"Laser {Amenity.GetWeaponRange()}r {Amenity.GetWeaponDamage()}d";
        }
    }
}
