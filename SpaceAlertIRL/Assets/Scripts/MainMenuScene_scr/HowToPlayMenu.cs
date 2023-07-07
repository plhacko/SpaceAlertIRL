using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    [SerializeField] GameObject[] Panels;
    int Iterator = 0;

    public void MoveRight()
    {
        if (Iterator >= Panels.Length - 1)
        { return; }

        Panels[Iterator].SetActive(false);
        Iterator++;
        Panels[Iterator].SetActive(true);
    }
    public void MoveLeft()
    {
        if (Iterator <= 0)
        { return; }

        Panels[Iterator].SetActive(false);
        Iterator--;
        Panels[Iterator].SetActive(true);

    }
}
