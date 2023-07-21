using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class DoorIcon : Icon
{
    [SerializeField]
    protected GameObject ActionPanelPrefab;

    private Door Door;
    private Room NextRoom; // this is the room, that the player is NOT in

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
            string _status = Door.IsOpen ? "open" : "close";
            GetComponentInChildren<TextMeshProUGUI>().text = $"{NextRoom.Name} : {_status}";

            if (Door.IsOpen)
            { GetComponent<Image>().color = Color.white; }
            else if (Door.IsLocked) { GetComponent<Image>().color = ProjectColors.NeonRed(); }
            else { GetComponent<Image>().color = ProjectColors.NeonYellow(); }
        }
    }

    public void SpawnActionPanel()
    {
        var actionPanel = GameObject.Find("ActionPanel");
        if (actionPanel == null)
        {
            // audio message
            AudioManager.Instance.PlaySentenceLoclaly("accessDenied_r actionPanelIsDisabled_r");
            return;
        }
        var actionPanelSpawner = actionPanel.GetComponent<ActionPanelSpawner>();

        actionPanelSpawner.ResetSelf();
        GameObject _go = Instantiate(ActionPanelPrefab, parent: actionPanel.transform);
        _go.GetComponent<DoorActionPanel>().Initialise(Door);
    }
}
