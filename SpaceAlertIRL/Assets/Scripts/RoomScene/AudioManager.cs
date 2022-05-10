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
    Queue<AudioClip> announcer_que = new Queue<AudioClip>(); // TODO: rename this

    private void Awake()
    {
        AudioSource_announcer = GetComponent<AudioSource>();

        SoundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in Sounds)
        {
            SoundDict.Add(sound.name, sound);
        }
    }


    public void PlaySentence(string sentence)
    {
        var splitted_sentence = sentence.Split();

        foreach (var w in splitted_sentence)
        {
            string word = w;
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            word = rgx.Replace(word, "");
            word = word.ToLower();

            if (word == "") continue;

            AudioClip s;
            if (SoundDict.ContainsKey(word))
            { s = SoundDict[word]; }
            else
            {
                s = SoundDict["wordMissing"];
                Debug.Log($"Word: \"{word}\" doesn't have audio file");
            }
            announcer_que.Enqueue(s);
        }
    }

    void Update()
    {
        if (AudioSource_announcer.isPlaying || announcer_que.Count == 0) { return; }
        AudioClip sound = announcer_que.Dequeue();

        AudioSource_announcer.clip = sound;
        // AudioSource_announcer.volume = sound.Volume;

        AudioSource_announcer.Play();
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            announcer_que.Clear();
        }
    }
}


[System.Serializable]
public class Sound
{
    public AudioClip AudioClip;
    public string Name { get => AudioClip.name; }

    [Range(0f, 1f)]
    public float Volume = 1f;
}