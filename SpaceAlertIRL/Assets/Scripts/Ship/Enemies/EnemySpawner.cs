#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemySpawner : MonoBehaviour, IOnServerFixedUpdate
{
    [SerializeField]
    List<GameObject> LightEnemies;
    [SerializeField]
    List<GameObject> Rockets;


    [SerializeField]
    public EnemyTimeStruct[] SpawnEnemyAtThisTime;
    private int SpawnEnemyAtThisTimeIndex = 0; // ?custom Enumerator?

#if SERVER
    float Timer;

    private void Start()
    {
        // sorts array by time   
        System.Array.Sort(SpawnEnemyAtThisTime, (x, y) => x.Time.CompareTo(y.Time));
    }

    // for now it will spawn every
    void IOnServerFixedUpdate.ServerFixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; }

        Timer += Time.deltaTime;

        // works with array SpawnEnemyAtThisTime
        // manages that all enemies are spawned at the right time
        while (SpawnEnemyAtThisTimeIndex < SpawnEnemyAtThisTime.Length && Timer > SpawnEnemyAtThisTime[SpawnEnemyAtThisTimeIndex].Time)
        {
            var enemyToSpawn = SpawnEnemyAtThisTime[SpawnEnemyAtThisTimeIndex];

            if (enemyToSpawn.Enemy == EnumOfEnemies.rndLightEnemy)
            { Debug.Log("not implemented rndLightEnemy"); }
            else
            { SpawnEnemy(enemyToSpawn.Enemy.ToString()); }

            SpawnEnemyAtThisTimeIndex++;
        }
    }

    public Enemy SpawnEnemy(GameObject e)
    {
        GameObject go = Instantiate(e, Vector3.zero, Quaternion.identity);

        go.GetComponent<NetworkObject>().Spawn();
        go.transform.SetParent(transform);
        Zone z = GetComponentInParent<Zone>();
        go.GetComponent<Enemy>().Initialise(z);
        GetComponentInParent<Zone>().UIActions.UpdateUI();

        // broadcast message for all clients
        string _zoneName = GetComponentInParent<Zone>().gameObject.name + "_r";
        GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient($"{_zoneName} enemyDetected_r", removeDuplicates: false);

        return go.GetComponent<Enemy>();
    }

    // TODO: could this be romoved?
    public Enemy SpawnEnemy(string enemyName)
    {
        foreach (var e in LightEnemies)
        {
            if (e.name == enemyName) { return SpawnEnemy(e); }
        }
        foreach (var e in Rockets)
        {
            if (e.name == enemyName) { return SpawnEnemy(e); }
        }

        Debug.Log($"Trying to spawn \"{enemyName}\", but it doesn't exist");
        return null;
    }

    public enum EnumOfEnemies { rndLightEnemy, Meteor, Fighter, Eye, Rocket }

    [System.Serializable]
    public struct EnemyTimeStruct
    {
        public EnumOfEnemies Enemy; // gameobject prefab
        public float Time;
    }

#endif
}


