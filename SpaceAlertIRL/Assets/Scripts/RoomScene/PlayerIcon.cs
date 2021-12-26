using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class PlayerIcon : MonoBehaviour
{
    private Player Player;
    private Action UpdateUIAction;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialise(Player player)
    {
        Player = player;

        UpdateUIAction = UpdateUI;
        UpdateUIAction();

        Player.CurrentRoomNameUIActions.AddAction(UpdateUIAction);
        //TODO: add one for player status
    }

    private void OnDisable()
    {
        if (Player != null)
        {
            Player.CurrentRoomNameUIActions.RemoveAction(UpdateUIAction);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateUI()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = $"{Player.Name.Value} : {Player.CurrentRoomName.Value} : {Player.Status}";
    }
}
