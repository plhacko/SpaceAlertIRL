using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using System;

public class RoomNameToText : MonoBehaviour
{
    TextMeshProUGUI RoomNameText, ZoneHPText, ZoneShieldText;

    [SerializeField]
    private Zone Zone;

    private Action UpdateUIAction;

    void Start()
    {
        RoomNameText = transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>();
        ZoneHPText = transform.Find("HP").GetComponentInChildren<TextMeshProUGUI>();
        ZoneShieldText = transform.Find("Shield").GetComponentInChildren<TextMeshProUGUI>();

        // writes the name of the current room to the GUI (of the local player)
        string _roomName = Player.GetLocalPlayer()?.CurrentRoomName.Value.ToString();

        RoomNameText.text = $"{_roomName}";
        Zone = GetZoneFromRoomName(_roomName); // getsZonme

        // updating UI using aciton onValueChangedependency
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Zone?.UIActions.AddAction(UpdateUIAction);

        Zone GetZoneFromRoomName(string roomName)
        {
            Room room = GameObject.Find(roomName).GetComponent<Room>();
            return room.GetComponentInParent<Zone>();
        }
    }

    protected void UpdateUI()
    {
        var _zoneHPText = Zone != null ? $"HP\n{Zone.HP}/{Zone.MaxHP}" : "___";
        var _zoneShieldText = Zone != null ? $"SH\n{Zone.GetShieldValue()}/{Zone.GetMaxShieldValue()}" : "___";

        ZoneHPText.text = _zoneHPText;
        ZoneShieldText.text = _zoneShieldText;

    }

    protected void OnDisable()
    {
        if (Zone != null)
        {
            Zone.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
