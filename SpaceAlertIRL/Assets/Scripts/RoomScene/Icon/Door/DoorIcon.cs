using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class DoorIcon : Icon
{
    private Door Door;
    private Room NextRoom; //this is the room, that the player is NOT in
    //rm private Action UpdateUIAction;


    override protected void OnDisable()
    {
        if (Door != null)
        {
            Door.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    // this works as a constructor
    public void Initialise(Door door, Room nextRoom)
    {
        Door = door;
        NextRoom = nextRoom;

        // UI update actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Door.UIActions.AddAction(UpdateUIAction);
    }

    override protected void UpdateUI()
    {
        if (Door != null && NextRoom != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"{NextRoom.Name} : {Door.Status}";

            if (Door.IsOpen.Value)
            { GetComponent<Image>().color = Color.green; }
            else { GetComponent<Image>().color = Color.white; }
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }

    public void SpawnActionPanel()
    {
        var actionPanel = GameObject.Find("ActionPanel");
        if (actionPanel == null)
        {
            // audio message
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySentenceLoclaly("accessDenied_r actionPanelIsDisabled_r");
            return;
        }
        var actionPanelSpawner = actionPanel.GetComponent<ActionPanelSpawner>();

        actionPanelSpawner.ResetSelf();
        GameObject _go = Instantiate(ActionPanelPrefab, parent: actionPanel.transform);
        _go.GetComponent<DoorActionPanel>().Initialise(Door);
    }
}
