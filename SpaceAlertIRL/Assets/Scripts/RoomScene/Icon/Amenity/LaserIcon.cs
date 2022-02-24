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
        // GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(Amenity); //TODO: rm

        var actionPanel = GameObject.Find("ActionPanel").GetComponent<ActionPanelSpawner>();
        
        actionPanel.ResetSelf();
        GameObject _go = Instantiate(ActionPanelPrefab, actionPanel.transform.position, actionPanel.transform.rotation, actionPanel.transform);
        _go.GetComponent<LaserActionPanel>().Initialise(Amenity);
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

    protected void OnDisable()
    {
        // removes the update action
        if (Amenity != null)
        { Amenity.UIActions.RemoveAction(UpdateUIAction); }
    }
}
