using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.NetworkVariable;
//rm using MLAPI.Messaging;
using Unity.Collections;

public class ServerDataContainer : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> myString = new NetworkVariable<FixedString32Bytes>("testText");
    public NetworkVariable<int> myInt = new NetworkVariable<int>(0);
    public NetworkVariable<int> myInt_Everyone = new NetworkVariable<int>(0);


    [ServerRpc(RequireOwnership = false)]
    public void AddServerRpc(ServerRpcParams rpcParams = default)
    {
        print("DEBUG: middleStartTime-" + Time.time.ToString());
        myInt.Value++;
        print("DEBUG: middleEndTime-" + Time.time.ToString());
    }

    [ServerRpc(RequireOwnership = false)]
    public void OneTimeChangeBackgroundColorServerRpc(ServerRpcParams rpcParams = default)
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        OneTimeChangeBackgroundColorClientRpc(new Color(r, g, b));
    }

    [ClientRpc]
    private void OneTimeChangeBackgroundColorClientRpc(Color c)
    {
        GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = c;
    }


    // TODO: this part should have its own *.cs file
    public AudioSource AudioSource;
    public List<AudioClip> ListOfAudioClips;

    [ServerRpc(RequireOwnership = false)]
    public void PlaySoundServerRpc(int index)
    {
        PlaySoundClientRpc(index);
    }

    [ClientRpc]
    public void PlaySoundClientRpc(int index)
    {
        if (index < 0 || index >= ListOfAudioClips.Count)
        { return; }
        // play a sound
        AudioSource.PlayOneShot(ListOfAudioClips[index]);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlaySoundBackServerRpc(ulong clientId, int soundIndex)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        PlaySoundBackClientRpc(soundIndex, clientRpcParams);
    }

    [ClientRpc]
    public void PlaySoundBackClientRpc(int index, ClientRpcParams clientRpcParams = default)
    {
        if (index < 0 || index >= ListOfAudioClips.Count)
        { return; }
        // play a sound
        AudioSource.PlayOneShot(ListOfAudioClips[index]);

        print("DEBUG-playsoundEND: time:" + Time.time);
    }



    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        ListOfAudioClips = new List<AudioClip>();
        for (int i = 1; i <= 7; i++)
        {
            string fileName = "dwarf_onClick_" + i.ToString();
            ListOfAudioClips.Add(Resources.Load<AudioClip>(fileName));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
