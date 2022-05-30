using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class SceneChanger : MonoBehaviour
{
    public string CurrentScene = "";

    public void ChangeScene(string sceneName)
    {
        try
        {
            if (CurrentScene != sceneName)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            CurrentScene = sceneName;
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
