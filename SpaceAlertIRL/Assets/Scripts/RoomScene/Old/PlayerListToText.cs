using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PlayerListToText : MonoBehaviour
{

    public List<Player> Players = new List<Player>(); // TODO: make private
    public TextMeshProUGUI Text;

    // Start is called before the first frame update
    void Start()
    {
        Text = this.GetComponent<TextMeshProUGUI>();

        foreach (Player player in Player.GetAllPlayers())
        {
            Players.Add(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Player player in Players)
        {
            sb.AppendLine($"{player.Name} : {player.CurrentRoomName.Value}");
        }
        Text.text = sb.ToString();
    }
}
