using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Icon : MonoBehaviour
{
    protected Action UpdateUIAction;

    [SerializeField]
    protected GameObject ActionPanelPrefab;

    protected abstract void UpdateUI();
    abstract protected void OnDisable();
}

abstract public class AmenityIcon<T> : Icon where T : Amenity
{
    protected T Amenity;
    
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

    public void SpawnActionPanel()
    {
        var actionPanel = GameObject.Find("ActionPanel").GetComponent<ActionPanelSpawner>();

        actionPanel.ResetSelf();
        GameObject _go = Instantiate(ActionPanelPrefab, actionPanel.transform.position, actionPanel.transform.rotation, actionPanel.transform);
        _go.GetComponent<ActionPanel<T>>().Initialise(Amenity);
    }
}
