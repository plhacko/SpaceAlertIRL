using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using System.Text.RegularExpressions;
using Unity.Netcode;
using Unity.Collections;

public class AudioManager : NetworkBehaviour
{
    public List<AudioClip> Sounds;
    Dictionary<string, AudioClip> SoundDict;

    AudioSource AudioSource_announcer;
    Queue<AudioClip> Announcer_que = new Queue<AudioClip>();

    private void Awake()
    {
        AudioSource_announcer = GetComponent<AudioSource>();

        SoundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in Sounds)
        {
            SoundDict.Add(sound.name, sound);
        }
    }


    public void RequestPlayingSentenceOnClient(FixedString32Bytes sentence, ulong clientId)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        PlaySentenceClientRpc(sentence.ToString(), clientRpcParams);
    }

    [ClientRpc]
    void PlaySentenceClientRpc(string sentence, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("I was here");

        AudioClip s;
        
        if (SoundDict.ContainsKey(sentence))
        { s = SoundDict[sentence]; }
        else
        { s = SoundDict["voiceTrackNotFound_r"]; }
        
        Announcer_que.Enqueue(s);
    }

    void FixedUpdate()
    {
        if (AudioSource_announcer.isPlaying || Announcer_que.Count == 0) { return; }

        AudioClip sound = Announcer_que.Dequeue();

        AudioSource_announcer.clip = sound;
        AudioSource_announcer.Play();
    }

    private void OnGUI() // TODO: rm whole
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Announcer_que.Clear();
        }
    }
}