using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public abstract class Amenity : NetworkBehaviour, IRestart
{
    [SerializeField]
    protected GameObject IconPrefab;
    public UpdateUIActions UIActions = new UpdateUIActions();
    public abstract void SpawnIconAsChild(GameObject parent);

    protected Room Room;

    protected virtual void Start()
    {
        Room = transform.parent.GetComponent<Room>();
        Room.Amenities.Add(this);
    }

    public abstract void Restart();
}

abstract public class Amenity<T> : Amenity where T : Amenity<T>
{
    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent: parent.transform);
        _go.GetComponent<AmenityIcon<T>>().Initialise((T)this);
    }
}
