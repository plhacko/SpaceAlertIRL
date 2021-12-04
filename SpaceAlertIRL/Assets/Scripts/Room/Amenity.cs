using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public abstract class Amenity : NetworkBehaviour
{
    string Name { get; }
}
