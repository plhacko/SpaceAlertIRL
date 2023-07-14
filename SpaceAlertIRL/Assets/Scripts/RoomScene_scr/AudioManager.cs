using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using System.Text.RegularExpressions;
using Unity.Netcode;
using Unity.Collections;


public class AudioManager : NetworkBehaviour, IRestart
{
    public bool Silent { get; private set; } = false;
    public void Mute(bool setSilent)
    {
        Silent = setSilent;
        if(Silent) { Announcer_que.Clear(); AudioSource_announcer.Stop(); }
    }
    public List<AudioClip> Sounds;
    Dictionary<string, AudioClip> SoundDict;

    AudioSource AudioSource_announcer;
    Queue<AudioClip> Announcer_que = new Queue<AudioClip>();

    List<string> LogMessages = new List<string>();
    NotificationMessagePanel _NotificationMessagePanel;
    NotificationMessagePanel NotificationMessagePanel
    {
        get
        {
            if (_NotificationMessagePanel == null)
            { _NotificationMessagePanel = GameObject.Find("NotificationMessagePanel")?.GetComponent<NotificationMessagePanel>(); }
            return _NotificationMessagePanel;
        }
    }


    private void Awake()
    {
        AudioSource_announcer = GetComponent<AudioSource>();

        SoundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in Sounds)
        {
            SoundDict.Add(sound.name, sound);
        }

        if (Instance == null)
            Instance = this;
    }
    public static AudioManager Instance { get; private set; }

    public void PlaySentenceLoclaly(string sentence, bool removeDuplicates = true)
    {
        // log the messgae in text format
        LogMessages.Add(sentence);
        NotificationMessagePanel?.InstatiateMessage(sentence);

        // silent check
        if (Silent) { return; }
        // play audio
        AudioClip audioClip;
        // sentence might contain multiple sentences
        foreach (string s in sentence.Split())
        {
            if (SoundDict.ContainsKey(s))
            { audioClip = SoundDict[s]; }
            else if (s == "") { continue; }
            else
            {
                audioClip = SoundDict["voiceTrackNotFound_r"];
                Debug.Log($"voicetrack: \"{s}\" is missing");
            }

            if (!removeDuplicates || !Announcer_que.Contains(audioClip))
            { Announcer_que.Enqueue(audioClip); }
        }
    }

    public void RequestPlayingSentenceOnClient(FixedString64Bytes sentence, bool removeDuplicates = true, ulong? clientId = null)
    {
        ClientRpcParams clientRpcParams;
        if (clientId != null) // broadcast to specific client
        {
            clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { clientId.Value } }
            };
        }
        else // broadcast to all clients
        { clientRpcParams = default; }

        PlaySentenceClientRpc(sentence.ToString(), removeDuplicates, clientRpcParams);
    }

    [ClientRpc]
    void PlaySentenceClientRpc(string sentences, bool removeDuplicates, ClientRpcParams clientRpcParams = default)
    {
        PlaySentenceLoclaly(sentences, removeDuplicates);
    }

    public void RequestVibratingSentenceOnClient(VibrationDuration millis, ulong? clientId = null)
        => RequestVibratingSentenceOnClient((long)millis, clientId);

    public void RequestVibratingSentenceOnClient(long millis, ulong? clientId = null)
    {
        ClientRpcParams clientRpcParams;
        if (clientId != null) // broadcast to specific client
        {
            clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { clientId.Value } }
            };
        }
        else // broadcast to all clients
        { clientRpcParams = default; }

        VibrateClientRpc(millis, clientRpcParams);
    }

    [ClientRpc]
    void VibrateClientRpc(long millis, ClientRpcParams clientRpcParams = default)
    {
        Vibration.Vibrate(millis);
    }


    void Update()
    {
        if (AudioSource_announcer.isPlaying || Announcer_que.Count == 0) { return; }

        AudioClip sound = Announcer_que.Dequeue();

        AudioSource_announcer.clip = sound;
        AudioSource_announcer.Play();
    }

    public void Restart()
    {
        LogMessages.Clear();
        Announcer_que.Clear();
    }
}