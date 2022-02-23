using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserIcon : AmenityIcon<Laser>
{
    public void ShootAtClosestEnemy()
    {
        throw new System.NotImplementedException();
    }

    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(Amenity);
    }

    protected override void UpdateUI()
    {
        if (Amenity != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"Laser {3}r {5}d"; //TODO: r = range, d = damage
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
