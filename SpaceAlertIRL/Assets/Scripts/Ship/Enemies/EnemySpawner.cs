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

    public void SpawnEnemy(GameObject e)
    {
        GameObject go = Instantiate(e);
        go.GetComponent<NetworkObject>().Spawn();
        go.transform.parent = transform;
        go.GetComponent<Enemy>().Initialise();
        GetComponentInParent<Zone>().UIActions.UpdateUI();
    }

#endif
}
