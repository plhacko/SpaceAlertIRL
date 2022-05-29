using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSetter : MonoBehaviour
{
    public void RequetsSettingLocalPlayerName()
    {
        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                string playerName = GetComponent<InputField>().text;
                player.RequetsSettingLocalPlayerName(playerName);
                return;
            }
        }
        Debug.Log("local player not found");
    }
}
