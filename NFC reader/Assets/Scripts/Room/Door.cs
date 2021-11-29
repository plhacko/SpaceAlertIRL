using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class Door : NetworkBehaviour
{
    public string Name { get => gameObject.name; }

    public Room RoomA;
    public Room RoomB;

    public string Status
    { //TODO: need to be finished up -> open/slosed/...
        get
        {
            return "placeholderStatus"; // placeholder TODO: remove placeholder
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AddSelfToRoom(RoomA);
        AddSelfToRoom(RoomB);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddSelfToRoom(Room r)
    {
        r.AddDoor(this);
    }

    public bool IsConnectedToRoom(Room r) // TODO: think about if this should be server RPC? -> ir would be dificult to make it so and it is most likely not needed // todo: more thinking
    {
        return r == RoomA || r == RoomB;
    }
}
