using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerDeadCheck : MonoBehaviour
{
    [SerializeField]
    GameObject[] DontUnload;

    // if the player is dead, this will deactivate most of the panels in UICanvas
    // so the player can't access it 
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
