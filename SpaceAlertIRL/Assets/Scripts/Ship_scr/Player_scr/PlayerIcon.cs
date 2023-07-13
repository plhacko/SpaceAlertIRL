using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerIcon : Icon
{
    private Player Player;

    TextMeshProUGUI HeaderText;

    private void Awake()
    {
        HeaderText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialise(Player player)
    {
        Player = player;

        UpdateUIAction = UpdateUI;
        UpdateUIAction();

        Player.UIActions.AddAction(UpdateUIAction);
        Player.UIActions_name.AddAction(UpdateUIAction);
    }

    override protected void OnDisable()
    {
        if (Player != null)
        {
            Player.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    override protected void UpdateUI()
    {
        HeaderText.text = $"{Player.Name} : {Player.CurrentRoomName.Value} : {Player.Status}";

        if (Player.IsDead)
        { GetComponent<Image>().color = ProjectColors.NeonRed(); }
    }
}
