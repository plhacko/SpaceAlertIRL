using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DoorActionPanel : MonoBehaviour
{
    public Door Door;
    private Action UpdateUIAction;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("RoomIcon_A").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomA : {Door.RoomA.Name}";
        transform.Find("RoomIcon_B").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomB : {Door.RoomB.Name}";

        // UI changing actions
        UpdateUIAction = UpdateUI;
        Door.IsOpenUIActions.AddAction(UpdateUIAction);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenDoor()
    {
        Door.OpenDoorServerRpc();
    }

    public void CloseDoor()
    {
        Door.CloseDoorServerRpc();
    }

    private void UpdateUI()
    {
        transform.Find("DoorStatus").GetComponentInChildren<TextMeshProUGUI>().text = $"Status : {Door.Status}";
    }

    private void OnDisable()
    {
        // removes the opdate action
        if (Door != null)
            Door.IsOpenUIActions.RemoveAction(UpdateUIAction);
    }

    // TODO: this will be repleaced with NFC
    public void RequestRoomChangeForCurrentPlayer()
    {
        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == MLAPI.NetworkManager.Singleton.LocalClientId)
            {
                if (player.CurrentRoomName.Value != Door.RoomA.Name)
                {
                    player.ChangeRoomServerRpc(Door.RoomA.Name);
                }
                else if (player.CurrentRoomName.Value != Door.RoomB.Name)
                {
                    player.ChangeRoomServerRpc(Door.RoomB.Name);
                }
                else
                { Debug.Log("player is not at the same room as doors and is trying to go through"); }
            }
        }

    }
}
