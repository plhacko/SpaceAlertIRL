using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject HostMenu;
    [SerializeField]
    GameObject ClientMenu;

    void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostMenu.SetActive(true);
            ClientMenu.SetActive(false);
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            ClientMenu.SetActive(true);
            HostMenu.SetActive(false);
        }
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();

        // Main menu will initialize all of those again so we need to clean those up
        var a = FindObjectsOfType<DontDestroyOnLoad>();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");

        foreach (var go in a)
        { Destroy(go.gameObject); }

    }
}
