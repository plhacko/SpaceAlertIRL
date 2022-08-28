using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using System;

public class RoomNameToText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI RoomNameText;
    [SerializeField]
    private TextMeshProUGUI ZoneHPText;
    [SerializeField]
    private Zone Zone;

    private Action UpdateUIAction;

    void Start()
    {
        RoomNameText = this.GetComponent<TextMeshProUGUI>();

        // writes the name of the current room to the GUI (of the local player)
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                string _roomName = player.CurrentRoomName.Value.ToString();

                RoomNameText.text = $"{_roomName}";
                Zone = GetZoneFromRoomName(_roomName); // getsZonme
                break;
                // TODO: if player object doesn't exist, there is a problem (make it a method)
            }
        }

        // updating UI using aciton onValueChangedependency
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Zone.UIActions.AddAction(UpdateUIAction);

        Zone GetZoneFromRoomName(string roomName)
        {
            Room room = GameObject.Find(roomName).GetComponent<Room>();
            return room.GetComponentInParent<Zone>();
        }
    }

    protected void UpdateUI()
    {
        if (Zone != null)
        {
            transform.Find("RoomHP").GetComponent<TextMeshProUGUI>().text = $"HP\n{Zone.HP}/{Zone.MaxHP}";
        }
        else
        {
            transform.Find("RoomHP").GetComponent<TextMeshProUGUI>().text = $"___"; // this happens when the player is using teleport
        } // TODO: smazat else
    }

    protected void OnDisable()
    {
        if (Zone != null)
        {
            Zone.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
