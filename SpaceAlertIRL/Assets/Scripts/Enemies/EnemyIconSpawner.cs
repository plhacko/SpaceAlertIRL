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
    }

    public void ChangeZone(Zone zone) => Zone = zone;

    // TODO: substutude this FixedUpdate for a action that happens when the number of enemis in the zone is changed
    private void FixedUpdate()
    {
        SpawnAllEnemies();
    }

    private Enemy[] EnemiesInZone = new Enemy[] { };
    private void SpawnAllEnemies()
    {
        Enemy[] _enemiesInZone = Zone.GenrateSortedEnemyArray();
        if (EnemiesInZone.SequenceEqual(_enemiesInZone)) { return; }
        EnemiesInZone = _enemiesInZone;

        // spawn enemy Icons
        ResetSelf();
        foreach (Enemy enemy in _enemiesInZone)
        {
            enemy.SpawnIconAsChild(gameObject);
        }
    }

    public void ResetSelf()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Text") { continue; }

            GameObject.Destroy(child.gameObject);
        }
    }

    protected override void OnDisable()
    { }
}
