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
        Player player;

        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                string _roomName = player.CurrentRoomName.Value.ToString();
                Room = GameObject.Find(_roomName).GetComponent<Room>();
                break;
                // TODO: if player object or the room doesn't exist, there is a problem (make it a ?method?)
            }
        }

        SpawnAllDoorIcons();
    }

    void SpawnAllDoorIcons()
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
