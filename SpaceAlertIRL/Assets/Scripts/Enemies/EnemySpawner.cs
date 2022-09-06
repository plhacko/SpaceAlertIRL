#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Netcode;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> LightEnemies;
    [SerializeField]
    List<GameObject> Rockets;

#if SERVER

    public Enemy SpawnEnemy(GameObject e)
    {
        GameObject go = Instantiate(e, Vector3.zero, Quaternion.identity);

        go.GetComponent<NetworkObject>().Spawn();
        go.transform.SetParent(transform);
        go.GetComponent<Enemy>().Initialise();
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

    public Enemy SpawnRnadomLightEmemy()
    {
        int i = Random.Range(0, LightEnemies.Count);
        return SpawnEnemy(LightEnemies[i]);
    }

#endif
}


