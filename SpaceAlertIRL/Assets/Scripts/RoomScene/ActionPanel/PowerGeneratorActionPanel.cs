using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;

public class PowerGeneratorActionPanel : MonoBehaviour
{
    public PowerGenerator PowerGenerator;
    private Action UpdateUIAction;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("RoomIcon_A").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomA : {Door.RoomA.Name}";
        transform.Find("RoomIcon_B").GetComponentInChildren<TextMeshProUGUI>().text = $"RoomB : {Door.RoomB.Name}";

        // UI changing actions
        UpdateUIAction = UpdateUI;
        PowerGenerator.UIActions.AddAction(UpdateUIAction);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateUI()
    {
        string a1 = PowerGenerator.EnergyPowerCellCount.Value.ToString();
        string a2 = PowerGenerator.EnergyStorage.Value.ToString();

    }

    private void OnDisable()
    {
        // removes the update action
        if (PowerGenerator != null)
        { PowerGenerator.UIActions.RemoveAction(UpdateUIAction); }
    }

    /*
    //TODO: DELETE
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

    }*/
}
