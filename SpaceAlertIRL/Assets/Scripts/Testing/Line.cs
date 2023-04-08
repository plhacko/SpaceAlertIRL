using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI.Extensions;

public class Line : MonoBehaviour
{
    [SerializeField] RectTransform rt1;
    [SerializeField] RectTransform rt2;
    UILineRenderer LR;

    void Start()
    {
        LR = GetComponent<UILineRenderer>();
        UpdateUI();
    }
    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        LR.Points = new Vector2[] { rt1.localPosition, rt2.localPosition };
        foreach (var p in LR.Points)
        {
            Debug.Log(p.ToString());
        }
        
        LR.SetAllDirty();
    }
}
