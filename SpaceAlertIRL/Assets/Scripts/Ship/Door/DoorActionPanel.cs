using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

public class DoorActionPanel : ActionPanel
{
    public Door Door;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("RoomIcon_A").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomA : {Door.RoomA.Name}";
        transform.Find("RoomIcon_B").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomB : {Door.RoomB.Name}";
    }

    virtual public void Initialise(Door door)
    {
        Door = door;

        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Door.UIActions.AddAction(UpdateUIAction);
    }

    public void OpenDoor()
    {
        Door.RequestOpenning();
    }

    public void CloseDoor()
    {
        Door.RequestClosing();
    }

    private void UpdateUI()
    {
        var _percentege = 100 * Door.OpenningClosingProgress.Value / Door.TimeToOpenDoorsConst;
        string _status = Door.IsOpen.Value ? "open" : "closed";

        transform.Find("DoorStatus").GetComponentInChildren<TextMeshProUGUI>().text = $"status : {_status}";
        transform.Find("DoorActivity").GetComponentInChildren<TextMeshProUGUI>().text = $"activity : {_percentege.ToString("0.##\\%")}";
    }

    protected override void OnDisable()
    {
        // removes the opdate action
        if (Door != null)
        { Door.UIActions.RemoveAction(UpdateUIAction); }
    }

    // TODO: this will be repleaced with NFC and for now is used for developement purpose
    public void RequestRoomChangeForCurrentPlayer()
    {
        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                if (player.CurrentRoomName.Value != Door.RoomA.Name)
                {
                    player.RequestChangingRoom(roomName: Door.RoomA.Name, conectToPanel: true, ignoreRestrictions: true);
                }
                else if (player.CurrentRoomName.Value != Door.RoomB.Name)
                {
                    player.RequestChangingRoom(Door.RoomB.Name, conectToPanel: true, ignoreRestrictions: true);
                }
                else
                { Debug.Log("player is not at the same room as doors and is trying to go through"); }
            }
        }
    }
}
