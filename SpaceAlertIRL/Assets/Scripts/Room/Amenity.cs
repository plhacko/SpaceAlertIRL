using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using System;

public abstract class Amenity : NetworkBehaviour
{
    [SerializeField]
    protected Room Room;

    protected UpdateUIActions UIActions = new UpdateUIActions();

    string Name { get; }
}
