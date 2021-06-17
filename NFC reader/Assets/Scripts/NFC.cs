using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class NFC : MonoBehaviour
{

	public string tagID;
	public Text tag_output_text;
	public bool tagFound = false;

	private AndroidJavaObject mActivity;
	private AndroidJavaObject mIntent;
	private string sAction;


	void Start()
	{
		tag_output_text.text = "Scan a NFC tag to make the cube disappear...";
	}

	void Update()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (!tagFound)
			{
				try
				{
					// Create new NFC Android object
					mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
					mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
					sAction = mIntent.Call<String>("getAction"); // resulte are returned in the Intent object
					if (sAction == "android.nfc.action.NDEF_DISCOVERED")
					{
						Debug.Log("Tag of type NDEF");
						tag_output_text.text = "DEBUG_1";

						
						AndroidJavaObject[] rawMsg = mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
						tag_output_text.text += "\nDEBUG_2 length:" + rawMsg.Length.ToString() + " name:" + rawMsg.ToString();

						AndroidJavaObject[] records = rawMsg[0].Call<AndroidJavaObject[]>("getRecords");
						tag_output_text.text += "\nDEBUG_3 length:" + records.Length.ToString() + " name:" + records.ToString();

						byte[] payLoad = records[0].Call<byte[]>("getPayload");
						tag_output_text.text += "\nDEBUG_4 length:" + payLoad.Length.ToString() + " name:" + payLoad.ToString();

						string result = System.Text.Encoding.Default.GetString(payLoad);
						tag_output_text.text += "\nDEBUG_5 length:" + result.Length.ToString() + " name:" + result;

					}
					/*
					else if (sAction == "android.nfc.action.TECH_DISCOVERED")
					{
						Debug.Log("TAG DISCOVERED");
						// Get ID of tag
						AndroidJavaObject mNdefMessage = mIntent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
						if (mNdefMessage != null)
						{
							byte[] payLoad = mNdefMessage.Call<byte[]>("getId");
							string text = System.Convert.ToBase64String(payLoad);
							tag_output_text.text += "This is your tag text: " + text;
							Destroy(GetComponent("MeshRenderer")); //Destroy Box when NFC ID is displayed
							tagID = text;
						}
						else
						{
							tag_output_text.text = "No ID found !";
						}
						tagFound = true;
						// How to read multiple tags maybe with this line mIntent.Call("removeExtra", "android.nfc.extra.TAG");
						return;
					}
					else if (sAction == "android.nfc.action.TAG_DISCOVERED")
					{
						Debug.Log("This type of tag is not supported !");
					}
					else
					{
						tag_output_text.text = "Scan a NFC tag to make the cube disappear...";
						return;
					}
					*/
				}
				catch (Exception ex)
				{
					string text = ex.Message;
					tag_output_text.text = text;
				}
			}
		}
	}
}

/** /


using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class NFC : MonoBehaviour {

	public string tagID;
	public Text tag_output_text;
	public bool tagFound = false;

	private AndroidJavaObject mActivity;
	private AndroidJavaObject mIntent;
	private string sAction;
    

    void Start() {
        tag_output_text.text = "Scan a NFC tag to make the cube disappear...";
    }

	void Update() {
		if (Application.platform == RuntimePlatform.Android) {
			if (!tagFound) {
				try {
					// Create new NFC Android object
					mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
					mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
					sAction = mIntent.Call<String>("getAction"); // resulte are returned in the Intent object
					if (sAction == "android.nfc.action.NDEF_DISCOVERED") {
						Debug.Log("Tag of type NDEF");
					}
					else if (sAction == "android.nfc.action.TECH_DISCOVERED") {
						Debug.Log("TAG DISCOVERED");
						// Get ID of tag
						AndroidJavaObject mNdefMessage = mIntent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
						if (mNdefMessage != null) {

                            AndroidJavaClass tmp = new AndroidJavaClass("MifareUltralight"); //.CallStatic<AndroidJavaObject>("get",mNdefMessage);
                            string text = tmp.CallStatic<string>("toString");
                            //byte[] payLoad = tmp.Call<byte[]>("readPages");
                            //string text = System.Convert.ToBase64String(payLoad);

                            // původní obsah
                            //string[] payLoad = mNdefMessage.Call<string[]>("getTechList");
                            //string text = string.Join("__",payLoad); // System.Convert.ToBase64String(payLoad);
                            tag_output_text.text += "This is your tag text: " + text; 
							Destroy (GetComponent("MeshRenderer")); //Destroy Box when NFC ID is displayed
							tagID = text;
						}
						else {
							tag_output_text.text = "No ID found !";
						}
						tagFound = true;
						// How to read multiple tags maybe with this line mIntent.Call("removeExtra", "android.nfc.extra.TAG");
						return;
					}
					else if (sAction == "android.nfc.action.TAG_DISCOVERED") {
						Debug.Log("This type of tag is not supported !");
					}
					else {
						tag_output_text.text = "Scan a NFC tag to make the cube disappear...";
						return;
					}
				}
				catch (Exception ex) {
					string text = ex.Message;
					tag_output_text.text = text;
				}
			}
		}
	}
}
/**/