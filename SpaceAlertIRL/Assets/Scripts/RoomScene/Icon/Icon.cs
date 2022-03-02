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
        GameObject _go = Instantiate(ActionPanelPrefab, parent:actionPanel.transform);
        _go.GetComponent<ActionPanel<T>>().Initialise(Amenity);
    }
}

abstract public class EnemyIcon<T> : Icon where T : Enemy
{
    protected T Enemy;

    public void Initialise(T enemy)
    {
        Enemy = enemy;
        UpdateUIAction = UpdateUI;
        UpdateUIAction += DestroyThisIconWithEnemyDeath;
        UpdateUIAction();
        Enemy.UIActions.AddAction(UpdateUIAction);
    }

    private void DestroyThisIconWithEnemyDeath()
    {
        if (Enemy.HP == 0)
        { Destroy(this.gameObject); }
    }

    override protected void OnDisable()
    {
        if (Enemy != null)
        {
            Enemy.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    public void SpawnInfoPanel()
    {
        throw new System.NotImplementedException();
    }
}
