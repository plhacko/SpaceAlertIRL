using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class DoorIconSpawner : MonoBehaviour
{
    [SerializeField]
    private Room Room;
    [SerializeField]
    private GameObject DoorIconPrefab;

    // Start is called before the first frame update
    void Start()
    {
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

        SpawnAllDoorIcons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnAllDoorIcons()
    {
        // remove old ones
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // spawns new door Objects
        foreach (Door d in Room.Doors)
        {
            GameObject _go = Instantiate(DoorIconPrefab, transform.position, transform.rotation, transform);
            if (Room != d.RoomA)
            { _go.GetComponent<DoorIcon>().Initialise(d, d.RoomA); }
            else
            { _go.GetComponent<DoorIcon>().Initialise(d, d.RoomB); }
        }
    }
}
