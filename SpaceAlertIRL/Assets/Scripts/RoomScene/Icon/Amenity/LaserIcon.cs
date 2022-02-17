using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserIcon : Icon
{
    [SerializeField]
    private Laser Laser;

    public void Initialise(Laser laser)
    {
        Laser = laser;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Laser.UIActions.AddAction(UpdateUIAction);
    }

    public void ShootAtClosestEnemy()
    {
        throw new System.NotImplementedException();
    }

    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(Laser);
    }

    protected override void OnDisable()
    {
        if (Laser != null)
        {
            Laser.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    protected override void UpdateUI()
    {
        if (Laser != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"Laser {3}r {5}d"; //TODO: r = range, d = damage
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
