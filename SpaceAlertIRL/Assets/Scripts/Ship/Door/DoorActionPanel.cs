using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;
using UnityEngine.UI;

public class DoorActionPanel : ActionPanel
{
    public Door Door;

    // UI
    TextMeshProUGUI DoorActivity, DoorStatus;
    Image OpenButton, CloseButton;

    private void Awake()
    {
        OpenButton = transform.Find("OpenButton").GetComponent<Image>();
        CloseButton = transform.Find("CloseButton").GetComponent<Image>();

        DoorStatus = transform.Find("DoorStatus").GetComponentInChildren<TextMeshProUGUI>();
        DoorActivity = transform.Find("DoorActivity").GetComponentInChildren<TextMeshProUGUI>();
    }

    virtual public void Initialise(Door door)
    {
        Door = door;

        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Door.UIActions.AddAction(UpdateUIAction);

        // is part of UI, but will ot change
        transform.Find("RoomIcon_A").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomA : {Door.RoomA.Name}";
        transform.Find("RoomIcon_B").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomB : {Door.RoomB.Name}";
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
        var _percentege = 100 * Door.OpenningClosingProgress / Door.TimeToOpenDoorsConst;
        string _status = Door.IsOpen ? "open" : "closed";

        DoorStatus.text = $"status : {_status}";
        DoorActivity.text = $"activity : {_percentege.ToString("0.##\\%")}";

        Color c;
        c = OpenButton.color;
        c.a = Door.IsOpen ? 0.6f : 1f;
        OpenButton.color = c;

        c = CloseButton.color;
        c.a = Door.IsOpen ? 1f : 0.6f;
        CloseButton.color = c;
    }

    protected override void OnDisable()
    {
        // removes the opdate action
        if (Door != null)
        { Door.UIActions.RemoveAction(UpdateUIAction); }
    }

    // TODO: this will be repleaced with NFC and for now is used for developement purpose
    public void RequestRoomChangeForLocalPlayer()
    {
        Player player = Player.GetLocalPlayer();

        if (player.CurrentRoomName.Value != Door.RoomA.Name)
        {
            player.RequestChangingRoom(roomName: Door.RoomA.Name, ignoreRestrictions: true);
        }
        else if (player.CurrentRoomName.Value != Door.RoomB.Name)
        {
            player.RequestChangingRoom(Door.RoomB.Name, ignoreRestrictions: true);
        }
        else
        { Debug.Log("player is not at the same room as doors and is trying to go through"); }

    }
}
