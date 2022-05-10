using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using System.Text.RegularExpressions;

public class AudioManager : MonoBehaviour
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

        // TODO: rm, just testing purpouses
        PlaySentence("");
        PlaySentence("notEnoughRockets_r");
    }


    public void PlaySentence(string sentence)
    {

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