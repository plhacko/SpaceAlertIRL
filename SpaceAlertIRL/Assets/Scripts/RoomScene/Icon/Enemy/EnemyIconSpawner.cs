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
}
