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

    void FixedUpdate()
    {
        // this script works nony on Android
        if (Application.platform != RuntimePlatform.Android) { return; }

        string newTagOutput = DetectNFCTag();
        if (newTagOutput == "debug")
        { TagOutput = newTagOutput; return; }

        if (newTagOutput != null && TagOutput != newTagOutput)
        {
            TagOutput = newTagOutput;
            RequestRoomChangeForCurrentPlayer(TagOutput);
        }
    }

    public void RequestRoomChangeForCurrentPlayer(string tagInfo)
    {
        Player.GetLocalPlayer()?.RequestChangingRoom(roomName: tagInfo);
    }

    string DetectNFCTag()
    {
        if (Application.platform != RuntimePlatform.Android) { return null; }
        try
        {
            // Create new NFC Android object
            AndroidJavaObject _mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
            AndroidJavaObject _mIntent = _mActivity.Call<AndroidJavaObject>("getIntent");
            string _sAction = _mIntent.Call<String>("getAction"); // results are returned in the Intent object

            if (_sAction == "android.nfc.action.NDEF_DISCOVERED")
            {
                AndroidJavaObject[] rawMsg = _mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
                AndroidJavaObject[] records = rawMsg[0].Call<AndroidJavaObject[]>("getRecords");
                byte[] payLoad = records[0].Call<byte[]>("getPayload");
                string result = System.Text.Encoding.Default.GetString(payLoad);

                result = result.Remove(0, 3); // removes information abouth language "en..."

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
