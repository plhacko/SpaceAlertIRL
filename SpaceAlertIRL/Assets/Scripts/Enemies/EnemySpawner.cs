#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Netcode;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour, IRestart
{
    [SerializeField]
    List<GameObject> Enemies;
    [SerializeField]
    List<GameObject> Rockets;
    [SerializeField]
    GameObject DistanceMeter;
    public GameObject GetDistanceMeter() => DistanceMeter;
    public void Restart()
    {
        foreach (var e in GetComponentsInChildren<Enemy>())
        {
            e.transform.GetComponent<NetworkObject>().Despawn();
        }
    }

#if SERVER

    public Enemy SpawnEnemy(GameObject enemy, bool silent = false)
    {
        GameObject go = Instantiate(enemy, Vector3.zero, Quaternion.identity);

        go.GetComponent<NetworkObject>().Spawn();
        go.transform.SetParent(transform);
        go.transform.localScale = Vector3.one; // reseting scale // during reparantig the scale sometimes changes
        go.GetComponent<Enemy>().Initialise();
        GetComponentInParent<Zone>().UIActions.UpdateUI();

        // broadcast message for all clients
        if (!silent)
        {
            string _zoneName = GetComponentInParent<Zone>().gameObject.name + "_r";
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient($"{_zoneName} enemyDetected_r", removeDuplicates: false);
        }
        return go.GetComponent<Enemy>();
    }

    public Enemy SpawnEnemy(string enemyName)
    {
        foreach (var e in Enemies)
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
        int i = Random.Range(0, Enemies.Count);
        return SpawnEnemy(Enemies[i]);
    }

#endif
}


