using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI.Extensions;
using UnityEngine.UIElements;

public class Line : MonoBehaviour
{
    // auxiliary points
    Transform p1 ,p2;

    UILineRenderer LR;

    void Awake()
    {
        p1 = transform.Find("p1");
        p2 = transform.Find("p2");

        LR = GetComponent<UILineRenderer>();
    }
    public void UpdateUI(Transform startPoint, Transform endPoint) => UpdateUI(startPoint.position, endPoint.position);
        public void UpdateUI(Vector3 startPoint, Vector3 endPoint)
    {
        if (startPoint.x < 0.1f || endPoint.x < 0.1f)
            return;

        p1.position = startPoint;
        p2.position = endPoint;
        LR.Points = new Vector2[] { p1.localPosition, p2.localPosition };
    }
}
