using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEditor;

public class EnemyEncounterPlanner : MonoBehaviour, IOnServerFixedUpdate, IRestart
{
    public EnemyTimeStruct[] EnemiesToSpawn;
    private int SpawnEnemyAtThisTimeIndex = 0; // ?custom Enumerator?
    float Timer = 0;

    private void Start()
    {
        // sorts array by time   
        System.Array.Sort(EnemiesToSpawn, (x, y) => x.SpawnTime.CompareTo(y.SpawnTime));

        ServerUpdater.Add(this.gameObject);

        // check if all zones exists
        foreach (var e in EnemiesToSpawn)
        {
            if (e.GetZoneObject() == null)
            {
                Debug.Log($"Zone : {e.ZoneName.ToString()} doesn't exit");
            }
        }
    }

    // for now it will spawn every
    void IOnServerFixedUpdate.ServerFixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; }

        Timer += Time.deltaTime;

        // works with array SpawnEnemyAtThisTime
        // manages that all enemies are spawned at the right time
        while (SpawnEnemyAtThisTimeIndex < EnemiesToSpawn.Length && Timer > EnemiesToSpawn[SpawnEnemyAtThisTimeIndex].SpawnTime)
        {
            // enemy to spawn
            var ets = EnemiesToSpawn[SpawnEnemyAtThisTimeIndex];

            if (ets.Enemy == EnumOfEnemies.rndLightEnemy)
            { ets.GetZoneObject().GetComponentInChildren<EnemySpawner>().SpawnRnadomLightEmemy(); }
            else
            {
                EnemySpawner enemySpawner = ets.GetZoneObject().GetComponentInChildren<EnemySpawner>();
                enemySpawner.SpawnEnemy(ets.Enemy.ToString());
            }

            SpawnEnemyAtThisTimeIndex++;
        }
    }

    // IRestart 
    public void Restart()
    {
        Destroy(gameObject);
    }

}
public enum ZoneNames { ZoneAlpha, ZoneBravo, ZoneCharlie, ZoneDelta, ZoneEcho, ZoneFoxtrod }
public enum EnumOfEnemies { rndLightEnemy, Meteor, Fighter, Eye, EnergyCloud, Rocket, Drednot }
[System.Serializable]
public struct EnemyTimeStruct
{
    public EnumOfEnemies Enemy; // gameobject prefab
    public float SpawnTime;
    public ZoneNames ZoneName;

    public GameObject GetZoneObject() => GameObject.Find(ZoneName.ToString());
}
