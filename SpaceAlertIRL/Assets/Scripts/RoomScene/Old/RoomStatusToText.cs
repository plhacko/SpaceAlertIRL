using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using Unity.Netcode;

public class RoomStatusToText : MonoBehaviour
{

    public TextMeshProUGUI Text;
    public Room Room; // TODO: make private

    // Start is called before the first frame update
    void Start()
    {
        Text = this.GetComponent<TextMeshProUGUI>();
        Player player;

        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                string _RoomName = player.CurrentRoomName.Value.ToString();
                Room = GameObject.Find(_RoomName).GetComponent<Room>();
                break;
                // TODO: if player object or the room doesn't exist, there is a problem (make it a ?method?)
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: pøedìlat na eventy (in future)
        Text.text = $"{WriteStatus()}\n{WriteDoors()}\n{WriteAmenities()}";
    }

    string WriteStatus() //TODO: 
    {
        return "Status: \n    * testing status 100% \n    * testing status 100% \n    * testing status 100% \n";
    }

    string WriteDoors()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Doors:");
        foreach (Door d in Room.Doors)
        {
            sb.AppendLine($"    * {d.Name} : {d.Status}");
        }

        return sb.ToString();
    }

    string WriteAmenities()
    {
        return "";
    }

}
