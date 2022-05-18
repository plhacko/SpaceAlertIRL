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
        // Zone.UIActions.AddAction(SpawnAllEnemies); // TODO: currently not needed TODO: rm?
    }

    private void FixedUpdate()
    {
        SpawnAllEnemies();
    }

    private Enemy[] EnemiesInZone = new Enemy[] { };
    private void SpawnAllEnemies()
    {
        Enemy[] _enemiesInZone = Zone.GenrateSortedEnemyArray(); //GetComponentsInChildren<Enemy>(); //TODO: rethink or remove // TODO: rm
        // Array.Sort(_enemiesInZone); // TODO rm
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
        // TODO: ?rm? - currently not needed
        // if (Zone != null)
        // { Zone.UIActions.RemoveAction(UpdateUIAction); }
    }
}
