using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActionPanel : MonoBehaviour
{
    protected Action UpdateUIAction;

    protected abstract void OnDisable();
}

public abstract class AmenityActionPanel<T> : ActionPanel where T : Amenity
{
    protected T Amenity;
    virtual public void Initialise(T t)
    {
        Amenity = t;

        // UI changing actions
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Amenity.UIActions.AddAction(UpdateUIAction);
    }

    protected override void OnDisable()
    {
        // removes the update action
        if (Amenity != null)
        { Amenity.UIActions.RemoveAction(UpdateUIAction); }
    }

    protected abstract void UpdateUI();
}
