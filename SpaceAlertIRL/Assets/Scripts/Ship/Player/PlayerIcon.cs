using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerIcon : Icon
{
    private Player Player;

    public void Initialise(Player player)
    {
        Player = player;

        UpdateUIAction = UpdateUI;
        UpdateUIAction();

        Player.CurrentRoomNameUIActions.AddAction(UpdateUIAction);
    }

    override protected void OnDisable()
    {
        if (Player != null)
        {
            Player.CurrentRoomNameUIActions.RemoveAction(UpdateUIAction);
        }
    }

    override protected void UpdateUI()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = $"{Player.Name.Value} : {Player.CurrentRoomName.Value} : {Player.Status}";
    }
}
