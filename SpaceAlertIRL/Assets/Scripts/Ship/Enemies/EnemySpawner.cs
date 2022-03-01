#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> LightEnemies;

    [SerializeField]
    float SpawnTimeConst = 10.0f;

#if SERVER
    float Timer;

    // for now it will spawn every
    void FixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) { return; }

        Timer += Time.deltaTime;
        while (Timer > SpawnTimeConst)
        {
            Timer -= SpawnTimeConst;

            if (LightEnemies.Count == 0) { throw new System.Exception("list of enemies is empty"); }
            int i = Random.Range(0, LightEnemies.Count);

            GameObject go = Instantiate(LightEnemies[i], transform.position, transform.localRotation, transform);
            go.GetComponent<NetworkObject>().Spawn();
            GetComponentInParent<Zone>().UIActions.UpdateUI();
        }
    }
#endif
}
