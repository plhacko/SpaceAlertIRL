using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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

    public void Restart()
    {
        Timer = 0;
    }
}

public enum EnumOfEnemies { rndLightEnemy, Meteor, Fighter, Eye, EnergyCloud, Rocket }
[System.Serializable]
public struct EnemyTimeStruct
{
    public EnumOfEnemies Enemy; // gameobject prefab
    public float SpawnTime;
    public Zone Zone;
}
