using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.NetworkVariable;
//rm using MLAPI.Messaging;

public class Room : NetworkBehaviour
{
    public string Name { get => gameObject.name; }

    public List<Door> Doors; // TODO: make {get; private set}
    public List<Amenity> Amenities;

    // the EnergySource will also be in the List of Amenities
    // note: there are derived classes form EnergyNode
    [SerializeField] //TODO: remove SerializeField
    private EnergyNode EnergySource;
    public EnergyNode GetEnergySource() => EnergySource;
    public void AddEnergySource(EnergyNode energySource)
    {
        if (EnergySource != null) { throw new System.Exception($"The EnergySource for this room \"{Name}\" can be added only once."); }
        EnergySource = energySource;
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
