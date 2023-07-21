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
    public bool SilentAudio { get; private set; } = false;
    public void Mute(bool setSilent)
    {
        SilentAudio = setSilent;
        if (SilentAudio) { AudioClip_que.Clear(); AudioSource_announcer.Stop(); }
    }
    public bool SilentVibrations { get; private set; } = false;
    public void MuteVibrations(bool setSilent)
    {
        SilentVibrations = setSilent;
    }

    public List<AudioClip> Sounds;
    Dictionary<string, AudioClip> SoundDict;

    AudioSource AudioSource_announcer;
    Queue<string> Sentence_que = new Queue<string>();
    Queue<AudioClip> AudioClip_que = new Queue<AudioClip>();

    List<string> LogMessages = new List<string>();

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
        NotificationMessagePanel.Instance?.InstatiateMessage(sentence);

        // silent check
        if (SilentAudio) { return; }

        if (!removeDuplicates || !Sentence_que.Contains(sentence))
        { Sentence_que.Enqueue(sentence); }
    }

    public void RequestPlayingSentenceOnClient(string sentence, bool removeDuplicates = true, ulong? clientId = null)
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

        PlaySentenceClientRpc(sentence, removeDuplicates, clientRpcParams);
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
        if (SilentVibrations)
        { return; }

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
        // waits for previous clip to finish playing
        if (AudioSource_announcer.isPlaying) { return; }

        // if entire sentence was played, loads the next one
        if (AudioClip_que.Count == 0)
        { TryToLoadNextSentence(); }

        // nothing was loaded check
        if (AudioClip_que.Count == 0) { return; }

        AudioClip sound = AudioClip_que.Dequeue();
        AudioSource_announcer.clip = sound;
        AudioSource_announcer.Play();

        bool TryToLoadNextSentence()
        {
            if (Sentence_que.Count == 0) { return false; }

            string sentence = Sentence_que.Dequeue();
            foreach (string s in sentence.Split())
            {
                AudioClip audioClip;
                if (SoundDict.ContainsKey(s))
                { audioClip = SoundDict[s]; }
                else if (s == "") { continue; }
                else
                {
                    audioClip = SoundDict["voiceTrackNotFound_r"];
                    Debug.Log($"voicetrack: \"{s}\" is missing");
                }

                AudioClip_que.Enqueue(audioClip);
            }

            return true;
        }
    }

    public void Restart()
    {
        LogMessages.Clear();
        AudioClip_que.Clear();
    }
}