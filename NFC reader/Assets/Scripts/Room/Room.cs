using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class Room : NetworkBehaviour, IRoom
{
    public string Name { get; }

    public List<Door> Doors;
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
}
