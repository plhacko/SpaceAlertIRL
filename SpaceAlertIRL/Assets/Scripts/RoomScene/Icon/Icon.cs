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
        var actionPanel = GameObject.Find("ActionPanel");
        if (actionPanel == null)
        {
            // audio message
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySentenceLoclaly("accessDenied_r actionPanelIsDisabled_r");
            return;
        }
        var actionPanelSpawner = actionPanel.GetComponent<ActionPanelSpawner>();

        actionPanelSpawner.ResetSelf();
        GameObject _go = Instantiate(ActionPanelPrefab, parent:actionPanel.transform);
        _go.GetComponent<AmenityActionPanel<T>>().Initialise(Amenity);
    }
}

abstract public class EnemyIcon<T> : Icon where T : Enemy
{
    protected T Enemy;

    public void Initialise(T enemy)
    {
        Enemy = enemy;
        UpdateUIAction = DestroyThisIconWithEnemyDeath;
        UpdateUIAction += UpdateUI;
        UpdateUIAction();
        Enemy.UIActions.AddAction(UpdateUIAction);
    }

    private void DestroyThisIconWithEnemyDeath()
    {
        if (Enemy == null)
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
