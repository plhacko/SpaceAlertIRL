using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerDeadCheck : MonoBehaviour
{
    [SerializeField]
    GameObject[] DontUnload;

    void Awake()
    {
        if (!Player.GetLocalPlayer().IsDead)
        { return; }

        // deactivate all
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != gameObject) { t.gameObject.SetActive(false); }
        }

        // reactivate some
        foreach (GameObject go in DontUnload)
        {
            go.SetActive(true);
        }
    }
}
