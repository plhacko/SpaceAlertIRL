using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class Room : NetworkBehaviour
{
    public string Name { get => gameObject.name; }

    public List<Door> Doors; // TODO: make {get; private set}
    public List<Amenity> Amenities;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetName() => Name;

    public void AddDoor(Door d)
    {
        // DEBUG // TODO: ??delete in final versin??
        if (Doors.Contains(d))
        { Debug.Log($"Trying to add doors twice - door: {d.Name}; room: {this.Name}"); return; }
        if (!d.IsConnectedToRoom(this))
        { Debug.Log($"Trying to add doors that aren't in the room - door: {d.Name}; room: {this.Name}"); return; }

        Doors.Add(d);
    }
}
