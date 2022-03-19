using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIconSpawner : ActionPanel
{
    Zone Zone;

    public void Initialise(Zone zone)
    {
        Zone = zone;
        UpdateUIAction = SpawnAllEnemies;
        UpdateUIAction();
        Zone.UIActions.AddAction(SpawnAllEnemies);
    }

    private void SpawnAllEnemies()
    {
        //TODO: add sorting mechanism, to sort the icons

        ResetSelf();

        foreach (Enemy enemy in Zone.GetEnemyList())
        {
            enemy.SpawnIconAsChild(gameObject);
        }
    }

    public void ResetSelf()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    protected override void OnDisable()
    {
        if (Zone != null)
        { Zone.UIActions.RemoveAction(UpdateUIAction); }
    }
}
