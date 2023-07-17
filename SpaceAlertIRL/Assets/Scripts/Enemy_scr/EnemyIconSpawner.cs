using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class EnemyIconSpawner : ActionPanel
{
    Zone Zone;

    [SerializeField]
    TextMeshProUGUI ZoneNameText;

    [SerializeField]
    GameObject DistanceMeter;
    public GameObject GetDistanceMeter() => DistanceMeter;
    public void Initialise(Zone zone)
    {
        Zone = zone;
        UpdateZoneNameText();
    }

    void FixedUpdate()
    {
        SpawnAllEnemies();
    }

    public void UpdateZoneNameText()
    {
        ZoneNameText.text = Zone.name;
    }
    public void ChangeZone(Zone zone)
    {
        Zone = zone;
        UpdateZoneNameText();
    }

    private Enemy[] EnemiesInZone = new Enemy[] { };
    private void SpawnAllEnemies()
    {
        Enemy[] enemiesInZone = Zone.GenrateSortedEnemyArray();
        if (EnemiesInZone.SequenceEqual(enemiesInZone))
        { return; }
        else
        { EnemiesInZone = enemiesInZone; }

        // spawn enemy Icons
        ResetSelf();
        foreach (Enemy enemy in enemiesInZone)
        {
            if (!enemy.IsDead)
            { enemy.SpawnIconAsChild(gameObject); }
        }
    }

    public void ResetSelf()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "HeaderTextPanel") { continue; }

            GameObject.Destroy(child.gameObject);
        }
    }

    protected override void OnDisable()
    { }
}
