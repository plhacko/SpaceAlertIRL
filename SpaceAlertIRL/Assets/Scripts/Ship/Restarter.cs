using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IRestart
{
    public void Restart();
}

public class Restarter : MonoBehaviour
{
    public void RestartGame()
    {
        RestartGameServerRpc();
    }

    [ServerRpc]
    private void RestartGameServerRpc()
    {
        IRestart[] objectsToRestart = GameObject.Find("ShipCanvas").GetComponentsInChildren<IRestart>();

        foreach (var r in objectsToRestart)
        {
            r.Restart();
        }

        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().ChangeScene("MenuScene");
    }
}
