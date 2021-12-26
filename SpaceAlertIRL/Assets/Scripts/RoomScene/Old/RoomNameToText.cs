using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class RoomNameToText : MonoBehaviour
{
    public TextMeshProUGUI Text; //TODO: make private

    // Start is called before the first frame update
    void Start()
    {
        Text = this.GetComponent<TextMeshProUGUI>();
        Player Player;

        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player = playerObject.GetComponent<Player>();
            if (Player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                Text.text = Player.CurrentRoomName.Value.ToString();
                break;
                // TODO: if player object doesn't exist, there is a problem (make it a method)
            }
        }
    }
}
