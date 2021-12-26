using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;




public class DoorIcon : MonoBehaviour
{
    private Door Door;
    private Room NextRoom; //this is the room, that the player is NOT in
    private Action UpdateUIAction;

    // Start is called before the first frame update
    void Start()
    {
        // done in Initialise
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable()
    {
        if (Door != null)
        {
            Door.IsOpenUIActions.RemoveAction(UpdateUIAction);
            Door.OpenningClosingProgressUIActions.RemoveAction(UpdateUIAction);
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
        Door.IsOpenUIActions.AddAction(UpdateUIAction);
        Door.OpenningClosingProgressUIActions.AddAction(UpdateUIAction);
    }

    public void SpawnActionPanel()
    {
        GameObject.Find("ActionPanel").GetComponent<ActionPanel>().DisplayThis(Door);
    }

    private void UpdateUI()
    {
        if (Door != null && NextRoom != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = $"{NextRoom.Name} : {Door.Status}";

            if (Door.IsOpen.Value)
            { GetComponent<Image>().color = Color.green; }
            else { GetComponent<Image>().color = Color.white; }
        }
        else
        { Debug.Log("Door or NextRoom were not given to DoorIcon"); } // TODO: smazat else
    }
}
