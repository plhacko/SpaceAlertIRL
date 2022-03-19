using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.NetworkVariable;
//rm using MLAPI.Messaging;
using System;

public abstract class ShipPart : NetworkBehaviour
{
    [SerializeField]
    protected GameObject IconPrefab;

    public abstract void SpawnIconAsChild(GameObject parent);

    public UpdateUIActions UIActions = new UpdateUIActions();
}
public abstract class Amenity : ShipPart
{
    protected Room Room;
    
    protected virtual void Start()
    {
        Room = transform.parent.GetComponent<Room>();
        Room.Amenities.Add(this);
    }
}

abstract public class Amenity<T> : Amenity where T : Amenity<T>
{
    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<AmenityIcon<T>>().Initialise((T)this);
    }
}
