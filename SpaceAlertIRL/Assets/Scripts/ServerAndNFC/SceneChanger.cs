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
            // TODO: give the player some information abouth invalidity of scene name (tag name)
            print("debug: invalid Scene");
        }

    }
    public void Exit()
    {
        Application.Quit();

    }
}
