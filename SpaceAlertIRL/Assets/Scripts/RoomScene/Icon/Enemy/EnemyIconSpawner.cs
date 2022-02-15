using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIconSpawner : MonoBehaviour
{
    Zone Zone;

    public void Initialise(Zone zone)
    {
        Zone = zone;
        SpawnAllEnemies();
    }

    private void SpawnAllEnemies()
    {
        //TODO: add restart -> if new enemy appears
        //TODO: add sorting mechanism, to sort the icons

        foreach (Enemy enemy in Zone.GetEnemyList())
        {
            enemy.SpawnIconAsChild(gameObject);
        }
    }

    void Start()
    {

    }

}
