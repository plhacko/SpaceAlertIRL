using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restarter : MonoBehaviour
{
    public void RestartGame()
    {
        IRestart[] objectsToRestart = GameObject.Find("ShipCanvas").GetComponentsInChildren<IRestart>();

        foreach (var r in objectsToRestart)
        {
            r.Restart();
        }
    }
}
