using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private ServerDataContainer ServerDataContainer;
    private Text UItext;
    private Text UItext_2;

    // Start is called before the first frame update
    void Start()
    {
        ServerDataContainer = GameObject.Find("ServerDataContainer").GetComponent<ServerDataContainer>();
        UItext = GameObject.Find("CountDownText").GetComponent<Text>();
        UItext_2 = GameObject.Find("CountDownText_2").GetComponent<Text>();

        UItext.text = ServerDataContainer.myInt.Value.ToString();
        UItext_2.text = ServerDataContainer.myInt_Everyone.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: maybe change to less often update
        UItext.text = ServerDataContainer.myInt.Value.ToString();
        UItext_2.text = ServerDataContainer.myInt_Everyone.Value.ToString();
    }

    // this will be triggered by a button
    public void Add()
    {
        print("DEBUG: startTime-" + Time.time.ToString());
        ServerDataContainer.AddServerRpc();
        print("DEBUG: midTime-" + Time.time.ToString());
        ServerDataContainer.myInt_Everyone.Value++;
        print("DEBUG: endTime-" + Time.time.ToString());
    }
}
