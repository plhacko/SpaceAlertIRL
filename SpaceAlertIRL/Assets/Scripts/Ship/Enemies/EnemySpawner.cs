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
    float SpawnTimeConst = 10.0f;

#if SERVER
    float Timer;

    // for now it will spawn every
    void IOnServerFixedUpdate.ServerFixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; }

        Timer += Time.deltaTime;
        while (Timer > SpawnTimeConst)
        {
            Timer -= SpawnTimeConst;

            if (LightEnemies.Count == 0) { throw new System.Exception("list of enemies is empty"); }
            int i = Random.Range(0, LightEnemies.Count);
            SpawnEnemy(LightEnemies[i]);
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

#endif
}
