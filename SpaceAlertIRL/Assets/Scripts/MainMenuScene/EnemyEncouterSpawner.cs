using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncouterSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject EnemyEncouterPrefab;

    public void SpawnEnemyEnounter()
    {
        GameObject _parent = GameObject.Find("ShipCanvas");

        var oldPlans = _parent.GetComponentsInParent<EnemyEncounterPlanner>();

        // destroy old plans
        foreach (var plan in oldPlans)
        {
            GameObject.Destroy(plan);
        }

        Instantiate(EnemyEncouterPrefab, parent: _parent.transform);
    }
}
