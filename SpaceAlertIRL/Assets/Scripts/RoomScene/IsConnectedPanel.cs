using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IsConnectedPanel : MonoBehaviour
{
    protected Action UpdateUIAction;
    [SerializeField]
    Player Player;

    private void Start()
    {
        Player = Player.GetLocalPlayer();

        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Player.UIActions.AddAction(UpdateUIAction);
    }

    protected void UpdateUI()
    {
        // GetComponentInChildren<TextMeshProUGUI>().text = Player.IsConnectedToPanel.Value ? "connected" : "disconnected";
    }

    private void OnDisable()
    {
        if (Player != null && UpdateUIAction != null)
        {
            Player.UIActions.RemoveAction(UpdateUIAction);
        }
    }
}
