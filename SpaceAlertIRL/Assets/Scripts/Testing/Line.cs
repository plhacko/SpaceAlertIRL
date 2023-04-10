using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI.Extensions;
using UnityEngine.UIElements;

public class Line : MonoBehaviour
{
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform EndPoint;

    // auxiliary points
    Transform p1 ,p2;

    UILineRenderer LR;

    void Awake()
    {
        p1 = transform.Find("p1");
        p2 = transform.Find("p2");

        LR = GetComponent<UILineRenderer>();
    }

    public void UpdateUI()
    {
        p1.position = StartPoint.position;
        p2.position = EndPoint.position;
        LR.Points = new Vector2[] { p1.localPosition, p2.localPosition };

        LR.SetAllDirty();
    }
}
