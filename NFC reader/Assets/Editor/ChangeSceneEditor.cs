using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NFC))]
public class NFCEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("ChangeScene"))
        {
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: CountDownRoom"))
        {
            (target as NFC).TagOutputSceneName = "CountDownRoom";
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: ChangingColorRoom"))
        {
            (target as NFC).TagOutputSceneName = "ChangingColorRoom";
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: MainRoom"))
        {
            (target as NFC).TagOutputSceneName = "MainRoom";
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: SoundRoom"))
        {
            (target as NFC).TagOutputSceneName = "SoundRoom";
            (target as NFC).ChangeScene();
        }
    }

}