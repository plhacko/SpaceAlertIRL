using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject[] OnlyHost;
    [SerializeField] GameObject[] OnlyClient;

    [SerializeField] GameObject MuteAudioButton;
    [SerializeField] GameObject MuteVibrationsButton;
    [SerializeField] GameObject MuteLogButton;
    [SerializeField] NotificationMessagePanel NotificationMessagePanel;

    private void OnEnable()
    {
        foreach (var go in OnlyHost)
        {
            go.SetActive(NetworkManager.Singleton.IsServer);
        }

        foreach (var go in OnlyClient)
        {
            go.SetActive(!NetworkManager.Singleton.IsServer);
        }
    }

    public void MuteAudio()
    {
        AudioManager am = AudioManager.Instance;
        bool setSilent = !am.Silent;
        am.Mute(setSilent);

        var text = MuteAudioButton.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = setSilent ? "Unmute audio" : "Mute audio";
        }
    }
    public void MuteVibrations()
    {
        AudioManager am = AudioManager.Instance;
        bool setSilent = !am.SilentVibrations;
        am.MuteVibrations(setSilent);

        var text = MuteVibrationsButton.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = setSilent ? "Unmute vibrations" : "Mute vibrations";
        }
    }
    public void MuteLog()
    {
        bool setSilent = !NotificationMessagePanel.Silent;
        NotificationMessagePanel.Mute(setSilent);

        var text = MuteLogButton.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = setSilent ? "Unmute log" : "Mute log";
        }
    }
}
