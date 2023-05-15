using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelSpawner : MonoBehaviour
{
    Color Color;

    public void ResetSelf()
    {
        GetComponent<Image>().color = Color;

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void HideActionPanel()
    {
        ResetSelf();
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    private void Start()
    {
        Color = GetComponent<Image>().color;
        ResetSelf();
    }

    private void OnDisable()
    {
        ResetSelf();
    }
}
