using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private ServerDataContainer ServerDataContaineer;
    private Text UItext;

    // Start is called before the first frame update
    void Start()
    {
        ServerDataContaineer = GameObject.Find("ServerDataContainer").GetComponent<ServerDataContainer>();
        UItext = GameObject.Find("CountDownText").GetComponent<Text>();

        UItext.text = ServerDataContaineer.myInt.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: maybe change to less often update
        UItext.text = ServerDataContaineer.myInt.Value.ToString();
    }

    // this will be triggered by a button
    public void Add()
    {
        print("DEBUG: startTime-" + Time.time.ToString());
        ServerDataContaineer.AddServerRpc();
        print("DEBUG: endTime-" + Time.time.ToString());
    }
}
