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

        string _RoomName = Player.GetLocalPlayer().CurrentRoomName.Value.ToString();
        Room = GameObject.Find(_RoomName).GetComponent<Room>();
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
            sb.AppendLine($"    * {d.Name} : {"d.Status"}");
        }

        return sb.ToString();
    }

    string WriteAmenities()
    {
        return "";
    }

}
