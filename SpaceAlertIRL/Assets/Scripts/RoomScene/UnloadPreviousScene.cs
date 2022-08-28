using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;


public class UnloadPreviousScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene s = SceneManager.GetSceneAt(0);

        if (s.name == "MainMenuScene") { SceneManager.UnloadSceneAsync(s); }
        Object.Destroy(this.gameObject);
    }
}
