using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NFC))]
public class NFCEditor : Editor
{
    // old code that was usefull for debuging one day
    /*
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("ChangeScene"))
        {
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: CountDownRoom"))
        {
            (target as NFC).TagOutput = "CountDownRoom";
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: ChangingColorRoom"))
        {
            (target as NFC).TagOutput = "ChangingColorRoom";
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: MainRoom"))
        {
            (target as NFC).TagOutput = "MainRoom";
            (target as NFC).ChangeScene();
        }

        if (GUILayout.Button("Scene: SoundRoom"))
        {
            (target as NFC).TagOutput = "SoundRoom";
            (target as NFC).ChangeScene();
        }
        if (GUILayout.Button("Scene: RoomScene"))
        {
            (target as NFC).TagOutput = "RoomScene";
            (target as NFC).ChangeScene();
        }
        if (GUILayout.Button("Scene: Ship_A"))
        {
            (target as NFC).TagOutput = "Ship_A";
            (target as NFC).ChangeScene();
        }
    }
    */
}