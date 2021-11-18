using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string CurrentScene = "";

    public void ChangeScene(string sceneName)
    {
        try
        {
            if (CurrentScene != sceneName) { SceneManager.LoadScene(sceneName); }
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
