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
        UpdateUIAction = SpawnAllEnemies;
        UpdateUIAction += UpdateZoneNameText;
        UpdateUIAction();
        Zone.UIActions.AddAction(UpdateUIAction);
    }
    public void UpdateZoneNameText()
    {
        ZoneNameText.text = Zone.name;
    }
    public void ChangeZone(Zone zone)
    {
        Zone.UIActions.RemoveAction(UpdateUIAction);
        Zone = zone;
        Zone.UIActions.AddAction(UpdateUIAction);

        UpdateUIAction();
    }

    private void SpawnAllEnemies()
    {
        Enemy[] _enemiesInZone = Zone.GenrateSortedEnemyArray();

        // spawn enemy Icons
        ResetSelf();
        foreach (Enemy enemy in _enemiesInZone)
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
    {
        Zone?.UIActions.RemoveAction(UpdateUIAction);
    }
}
