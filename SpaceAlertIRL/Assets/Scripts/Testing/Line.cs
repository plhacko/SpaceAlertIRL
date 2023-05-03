using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI.Extensions;
using UnityEngine.UIElements;

public class Line : MonoBehaviour
{
    // auxiliary points
    Transform p1, p2;

    UILineRenderer LR;


    void Awake()
    {
        p1 = transform.Find("p1");
        p2 = transform.Find("p2");

        LR = GetComponent<UILineRenderer>();
        LR.color = LR.color - new Color(0, 0, 0, 1);
    }
    public void UpdateUI(Transform startPoint, Transform endPoint) => UpdateUI(startPoint.position, endPoint.position);
    public void UpdateUI(Vector3 startPoint, Vector3 endPoint)
    {
        p1.position = startPoint;
        p2.position = endPoint;

        Vector3 v1 = transform.InverseTransformPoint(startPoint);
        Vector3 v2 = transform.InverseTransformPoint(endPoint);

        // TODO: rm p1 and p2 (even from the prefab)
        // LR.Points = new Vector2[] { p1.localPosition, p2.localPosition };
        LR.Points = new Vector2[] { v1, v2 };

        // makes the line slowly appear
        LR.color = LR.color + Time.deltaTime * new Color(0, 0, 0, 0.42f);
    }
}
