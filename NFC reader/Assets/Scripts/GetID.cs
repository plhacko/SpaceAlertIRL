using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: this part is propably old, I should try to replace it with MLAPI solution
using System.Net;
using System.Net.Sockets;

public class GetID : MonoBehaviour
{
    public Text id_text;

    // Start is called before the first frame update
    void Start()
    {
        id_text.text = "IP: " + LocalIPAddress();

        

    }


    // TODO: this method is propably old, I should try to replace it with MLAPI solution
    // also this method works only for some phones
    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
