using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NFC : MonoBehaviour
{
    public string TagOutput = "";

    const float TimetoDisconectConst = 3.0f;
    float DisconectTimer = TimetoDisconectConst;

    void FixedUpdate()
    {
        // this script works nony on Android
        // if (Application.platform != RuntimePlatform.Android) { return; }

        string newTagOutput = DetectNFCTag();

        if (newTagOutput != null)
        {
            TagOutput = newTagOutput;
            RequestRoomChangeForCurrentPlayer(TagOutput);

            DisconectTimer = TimetoDisconectConst;
            GameObject.Find("ActionPanel")?.SetActive(true);

            GameObject.Find("DebugTextLog").GetComponent<TextMeshProUGUI>().text = "tag found"; // TODO: rm
        }

        DisconectTimer -= Time.deltaTime;
        if (DisconectTimer <= 0.0f)
        {
            GameObject.Find("ActionPanel")?.SetActive(false);

            GameObject.Find("DebugTextLog").GetComponent<TextMeshProUGUI>().text = "tag not found"; // TODO: rm
        }
    }

    public void RequestRoomChangeForCurrentPlayer(string roomName)
    {
        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                player.RequestChangingRoom(roomName);
            }
        }
    }

    string DetectNFCTag()
    {
        if (Application.platform != RuntimePlatform.Android) { return null; } // TODO: rm
        try
        {
            // Create new NFC Android object
            AndroidJavaObject _mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
            AndroidJavaObject _mIntent = _mActivity.Call<AndroidJavaObject>("getIntent");
            string _sAction = _mIntent.Call<String>("getAction"); // results are returned in the Intent object

            // _mActivity.Call("finish");

            if (_sAction == "android.nfc.action.NDEF_DISCOVERED")
            {
                AndroidJavaObject[] rawMsg = _mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
                AndroidJavaObject[] records = rawMsg[0].Call<AndroidJavaObject[]>("getRecords");
                byte[] payLoad = records[0].Call<byte[]>("getPayload");
                string result = System.Text.Encoding.Default.GetString(payLoad);

                result = result.Remove(0, 3); // removes information abouth language "en..."


                // TODO: rm
                // TagOutput = result;
                // RequestRoomChangeForCurrentPlayer(result);

                return result;
            }
        }
        catch (Exception ex)
        {
            string text = ex.Message;
            Debug.LogError(text);
        }

        return null;
    }
}
