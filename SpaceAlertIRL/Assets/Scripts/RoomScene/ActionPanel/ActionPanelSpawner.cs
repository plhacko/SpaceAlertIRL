using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelSpawner : MonoBehaviour
{
    public void ResetSelf()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void Start()
    {
        ResetSelf();
    }

    private void OnDisable()
    {
        ResetSelf();
    }
}
