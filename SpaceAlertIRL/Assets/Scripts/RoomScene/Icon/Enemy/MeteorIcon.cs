using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeteorIcon : Icon
{
    [SerializeField]
    private Meteor Meteor;

    public void Initialise(Meteor meteor)
    {
        Meteor = meteor;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Meteor.UIActions.AddAction(UpdateUIAction);
    }

    protected override void OnDisable()
    {
        if (Meteor != null)
        {
            Meteor.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    protected override void UpdateUI()
    {
        if (Meteor != null)
        {
            string line1 = $"Meteor, HP : {Meteor.HP}/{Meteor.MaxHP}, ES : {Meteor.EnergyShields}/{Meteor.MaxEnergyShields}";
            string line2 = $"Damage: {Meteor.HP}";
            string line3 = $"Impact in : {Meteor.Impact}";
            GetComponentInChildren<TextMeshProUGUI>().text = line1 + '\n' + line2 + '\n' + line3;
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
