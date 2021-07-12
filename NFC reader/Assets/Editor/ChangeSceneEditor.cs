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
    }

}