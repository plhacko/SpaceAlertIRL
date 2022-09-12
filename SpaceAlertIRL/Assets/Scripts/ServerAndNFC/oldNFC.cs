using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


// !!! not used !!!
// just not to thow away the commented part
public class oldNFC : MonoBehaviour
{
    public string TagOutput = "";
    public bool TagFound = false;

    private AndroidJavaObject _mActivity;
    private AndroidJavaObject _mIntent;
    private string _sAction;

    [SerializeField]
    Player LocalPlayer;

    private SceneChanger _sceneChanger;

    private void Start()
    {
        _sceneChanger = GetComponent<SceneChanger>();
    }

    public void ChangeScene() //TODO: rm?
    {
        _sceneChanger.ChangeScene(TagOutput);
    }

    public void RequestRoomChangeForCurrentPlayer(string roomName)
    {
        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                // player.RequestChangingRoom(roomName); // old 
            }
        }
    }

    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!TagFound)
            {
                try
                {
                    // Create new NFC Android object
                    _mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
                    _mIntent = _mActivity.Call<AndroidJavaObject>("getIntent");
                    _sAction = _mIntent.Call<String>("getAction"); // results are returned in the Intent object
                    if (_sAction == "android.nfc.action.NDEF_DISCOVERED")
                    {
                        // Debug.Log("Tag of type NDEF"); // TODO: delete this line
                        AndroidJavaObject[] rawMsg = _mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
                        AndroidJavaObject[] records = rawMsg[0].Call<AndroidJavaObject[]>("getRecords");
                        byte[] payLoad = records[0].Call<byte[]>("getPayload");
                        string result = System.Text.Encoding.Default.GetString(payLoad);

                        result = result.Remove(0, 3); // removes information abouth language "en..."

                        // ChangeScene(); //TODO: rm
                        TagOutput = result;
                        RequestRoomChangeForCurrentPlayer(result);
                    }
                    /* // TODO: rm
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
                    Debug.LogError(text);
                    // tag_output_text = text; //TODO: if an exception would happen, it would be good to somehow tell the user
                }
            }
        }
    }
}

// TODO: rm
// it was a solution that did'n work for me, but I based my solution on it
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

                            // pùvodní obsah
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
