using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyEncounterPlanner : MonoBehaviour, IOnServerFixedUpdate
{
    public EnemyTimeStruct[] EnemiesToSpawn;
    private int SpawnEnemyAtThisTimeIndex = 0; // ?custom Enumerator?
    float Timer;

    private void Start()
    {
        // sorts array by time   
        System.Array.Sort(EnemiesToSpawn, (x, y) => x.SpawnTime.CompareTo(y.SpawnTime));
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
            { ets.Zone.GetComponentInChildren<EnemySpawner>().SpawnRnadomLightEmemy(); }
            else
            {
                EnemySpawner enemySpawner = ets.Zone.GetComponentInChildren<EnemySpawner>();
                enemySpawner.SpawnEnemy(ets.Enemy.ToString());
            }

            SpawnEnemyAtThisTimeIndex++;
        }
    }
}

public enum EnumOfEnemies { rndLightEnemy, Meteor, Fighter, Eye, Rocket }
[System.Serializable]
public struct EnemyTimeStruct
{
    public EnumOfEnemies Enemy; // gameobject prefab
    public float SpawnTime;
    public Zone Zone;
}
