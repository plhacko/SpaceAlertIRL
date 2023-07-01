using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameCountDownToText : MonoBehaviour
{
    protected Action UpdateUIAction;
    [SerializeField]
    MissionTimer MissionTimer;

    private void Start()
    {
        MissionTimer = GameObject.Find("MissionTimer").GetComponent<MissionTimer>();

        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        MissionTimer.UIActions.AddAction(UpdateUIAction);
    }

    protected void UpdateUI()
    {
        int timeToEnd = (int)MissionTimer.GetTimeToEnd();

        GetComponentInChildren<TextMeshProUGUI>().text = $"{timeToEnd / 60}m {timeToEnd % 60}s";
    }

    private void OnDisable()
    {
        if (MissionTimer != null && UpdateUIAction != null)
        {
            MissionTimer.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
