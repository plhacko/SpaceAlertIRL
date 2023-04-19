using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    GameObject Victory;
    [SerializeField]
    GameObject Defeat;

    void Start()
    {
        if (IsDead())
        {
            Victory.SetActive(false);
            Defeat.SetActive(true);
        }
        else 
        {
            Victory.SetActive(true);
            Defeat.SetActive(false);
        }
    }

    private bool IsDead()
    {
        Zone[] Zones = GameObject.Find("ShipCanvas").GetComponentsInChildren<Zone>();

        foreach (Zone z in Zones)
        {
            if (z.HP <= 0)
            { return true; }
        }
        return false;
    }
}
