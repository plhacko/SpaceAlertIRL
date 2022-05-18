using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyIconSpawner : ActionPanel
{
    [SerializeField]
    Zone Zone;

    public void Initialise(Zone zone)
    {
        Zone = zone;
        UpdateUIAction = SpawnAllEnemies;
        UpdateUIAction();
        Zone.UIActions.AddAction(SpawnAllEnemies);
    }

    private void FixedUpdate()
    {
        SpawnAllEnemies();
    }

    private Enemy[] EnemiesInZone = new Enemy[] { };
    private void SpawnAllEnemies()
    {
        Enemy[] _enemiesInZone = Zone.GetComponentsInChildren<Enemy>(); //TODO: rethink or remove
        Array.Sort(_enemiesInZone);
        if (EnemiesInZone.SequenceEqual(_enemiesInZone)) { return; }
        EnemiesInZone = _enemiesInZone;

        // spawn them
        ResetSelf();
        foreach (Enemy enemy in _enemiesInZone) //enemiesInZone) //Zone.GetEnemyList())
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
