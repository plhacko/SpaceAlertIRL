using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    ServerDataContainer ServerDataContainer;
    List<AudioClip> ListOfAudioClips;
    // Start is called before the first frame update
    void Start()
    {
        ServerDataContainer = GameObject.Find("ServerDataContainer").GetComponent<ServerDataContainer>();

        // TODO: will be needed later (or not)
        /*/
        ListOfAudioClips = new List<AudioClip>();

        for (int i = 1; i <= 7; i++)
        {
            string fileName = "dwarf_onClick_" + i.ToString();
            ListOfAudioClips.Add(Resources.Load<AudioClip>(fileName));
        }
        /**/
    }

    public void PlayOneClip(int i)
    {
        ServerDataContainer.PlaySoundServerRpc(i);
    }

    public void PlayOneClip_2(int i)
    {
        ServerDataContainer.PlaySoundClientRpc(i);
    }

    public void PlayOneClip_3(int i)
    {
        ulong u = MLAPI.NetworkManager.Singleton.LocalClientId;
        print("DEBUG-playsoundSTART: clientID:" + u + "; time:" + Time.time);
        ServerDataContainer.PlaySoundBackServerRpc(u, i);
    }
}
