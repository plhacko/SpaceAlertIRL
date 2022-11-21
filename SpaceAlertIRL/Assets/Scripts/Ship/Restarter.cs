using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRestart
{
    public void Restart();
}

public class Restarter : MonoBehaviour
{
    public void RestartGame()
    {
        IRestart[] objectsToRestart = GameObject.Find("ShipCanvas").GetComponentsInChildren<IRestart>();

        foreach (var r in objectsToRestart)
        {
            r.Restart();
        }

        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().ChangeScene("MainMenuScene");
    }
}
