using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserActionPanel : MonoBehaviour
{
    Laser Laser;
    private Action UpdateUIAction;

    public void Initialise(Laser laser)
    {
        Laser = laser;

        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Laser.UIActions.AddAction(UpdateUIAction);
    }

    private void UpdateUI()
    {
        var _damage = Laser.Damage.Value;
        var _range = Laser.Range.Value;
        var _heat = Laser.Heat.Value;

        transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>().text = "Status : good"; // TODO: redo this
        transform.Find("Damage").GetComponentInChildren<TextMeshProUGUI>().text = $"Damage : {_damage}";
        transform.Find("Range").GetComponentInChildren<TextMeshProUGUI>().text = $"Range : {_range}";
        transform.Find("Heat").GetComponentInChildren<TextMeshProUGUI>().text = $"Heat : {_heat}";
    }

    private void OnDisable()
    {
        // removes the update action
        if (Laser != null)
        { Laser.UIActions.RemoveAction(UpdateUIAction); }
    }
}
