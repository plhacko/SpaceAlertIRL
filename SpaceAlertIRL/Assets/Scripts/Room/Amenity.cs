using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.NetworkVariable;
//rm using MLAPI.Messaging;
using System;

public abstract class Amenity : NetworkBehaviour
{
    [SerializeField]
    protected Room Room;

    [SerializeField]
    protected GameObject IconPrefab;
    public abstract void SpawnIconAsChild(GameObject parent);

    public UpdateUIActions UIActions = new UpdateUIActions();

    protected virtual void Start()
    {
        Room.Amenities.Add(this);
    }

    string Name { get; }
}
