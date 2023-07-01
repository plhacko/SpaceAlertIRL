using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Linq;

enum UnloadPreviousSceneEnum_options { unloadMentioned, unloadUnmentioned }

public class UnloadPreviousScene : MonoBehaviour
{
    [SerializeField]
    UnloadPreviousSceneEnum_options options = UnloadPreviousSceneEnum_options.unloadMentioned;

    [SerializeField]
    string[] SceneNames;

    // Start is called before the first frame update
    void Start()
    {
        List<Scene> scenesToBeUnloaded = new List<Scene>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            switch (options)
            {
                case UnloadPreviousSceneEnum_options.unloadMentioned:
                    if (SceneNames.Contains(s.name)) { scenesToBeUnloaded.Add(s); }
                    break;
                case UnloadPreviousSceneEnum_options.unloadUnmentioned:
                    if (!SceneNames.Contains(s.name)) { scenesToBeUnloaded.Add(s); }
                    break;
            }
        }

        foreach (Scene s in scenesToBeUnloaded)
        {
            SceneManager.UnloadSceneAsync(s);
        }

        Object.Destroy(this.gameObject);
    }
}
