using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSetter : MonoBehaviour
{
    public void RequetsSettingLocalPlayerName()
    {
        string playerName = GetComponent<InputField>().text;
        Player.GetLocalPlayer()?.RequetsSettingLocalPlayerName(playerName);
    }
}
