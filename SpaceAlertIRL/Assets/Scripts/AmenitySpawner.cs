using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AmenitySpawner : MonoBehaviour
{
    [SerializeField]
    private Room Room;

    // Start is called before the first frame update
    void Start()
    {
        string _roomName = Player.GetLocalPlayer().CurrentRoomName.Value.ToString();
        Room = GameObject.Find(_roomName).GetComponent<Room>();

        SpawnAllAmenityIcons();
    }

    void SpawnAllAmenityIcons()
    {
        // remove old ones
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // spawns new door Objects
        foreach (Amenity a in Room.Amenities)
        {
            a.SpawnIconAsChild(transform.gameObject);
        }
    }
}
