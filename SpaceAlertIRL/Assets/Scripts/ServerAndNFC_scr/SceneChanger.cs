using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class SceneChanger : MonoBehaviour
{
    public string CurrentSceneName { get => SceneManager.GetActiveScene().name; }

    public void ChangeScene(string sceneName)
    {
        try
        {
            if (CurrentSceneName != sceneName)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
        catch (System.Exception)
        {
            print("invalid Scene");
        }

    }

    public void StartGame()
    {
        GameObject.Find("Restarter").GetComponent<Restarter>().RestartAllPlayers();
        //// ChangeScene("RoomScene");
    }

    public void Exit()
    {
        Application.Quit();

    }
}
