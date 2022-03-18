using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActionPanel : MonoBehaviour
{
    protected Action UpdateUIAction;
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

    private void OnDisable()
    {
        // removes the update action
        if (Amenity != null)
        { Amenity.UIActions.RemoveAction(UpdateUIAction); }
    }

    protected abstract void UpdateUI();
}
