using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class EndScreenText : MonoBehaviour
{
    void Start()
    {
        Zone[] Zones = GameObject.Find("ShipCanvas").GetComponentsInChildren<Zone>();

        StringBuilder sb = new StringBuilder();

        foreach (Zone z in Zones)
        {
            sb.AppendLine($"{z.name} HP : {z.HP}/{z.MaxHP}");
        }
        sb.AppendLine();

        sb.AppendLine("Players:");
        foreach (Player p in Player.GetAllPlayers())
        {
            sb.AppendLine($"{p.Name} : {p.Status}");
        }

        GetComponentInChildren<TextMeshProUGUI>().text = sb.ToString();   
    }
}
