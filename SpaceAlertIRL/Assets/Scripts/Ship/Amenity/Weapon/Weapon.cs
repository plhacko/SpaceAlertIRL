using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon<T> : Amenity where T : Weapon<T>
{
    public Zone Zone { get; private set; }

    protected override void Start()
    {
        base.Start();
        Zone = GetComponentInParent<Zone>();
    }

    public override void SpawnIconAsChild(GameObject parent)
    {
        GameObject _go = Instantiate(IconPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
        _go.GetComponent<AmenityIcon<T>>().Initialise((T)this);
    }
}

