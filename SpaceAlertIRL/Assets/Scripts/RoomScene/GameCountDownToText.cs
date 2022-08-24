using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameCountDownToText : MonoBehaviour
{
    protected Action UpdateUIAction;
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

        GetComponentInChildren<TextMeshProUGUI>().text = $"{timeToEnd/60}m {timeToEnd%60}s";
    }

    private void OnDisable()
    {
        MissionTimer.UIActions.RemoveAction(UpdateUIAction);
    }
}
