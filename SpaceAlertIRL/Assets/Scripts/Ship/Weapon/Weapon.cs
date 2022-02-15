using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Amenity
{
    public Zone Zone;

    protected override void Start()
    {
        base.Start();

        // TODO: might fail
        Zone = GetComponentInParent<Zone>();
    }

}
