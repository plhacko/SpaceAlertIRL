using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Icon : MonoBehaviour
{
    protected Action UpdateUIAction;
    
    protected abstract void UpdateUI();
    abstract protected void OnDisable();
}

abstract public class AmenityIcon<T> : Icon where T : Amenity
{
    protected T Amenity;
    
    [SerializeField]
    protected GameObject ActionPanelPrefab;

    public void Initialise(T amenity)
    {
        Amenity = amenity;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Amenity.UIActions.AddAction(UpdateUIAction);
    }

    override protected void OnDisable()
    {
        if (Amenity != null)
        {
            Amenity.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
