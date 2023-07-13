using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI.Extensions;
using UnityEngine.UIElements;

public class Line : MonoBehaviour
{
    UILineRenderer LR;
    [SerializeField] bool AppearSlowly = true;
    [SerializeField] float LineAppearSpeed;
    void Awake()
    {
        LR = GetComponent<UILineRenderer>();
        if (AppearSlowly)
            LR.color = LR.color - new Color(0, 0, 0, 1);
    }
    public void UpdateUI(Transform startPoint, Transform endPoint) => UpdateUI(startPoint.position, endPoint.position);
    public void UpdateUI(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 localStartPoint = transform.InverseTransformPoint(startPoint);
        Vector3 localEndlocalEndPoint = transform.InverseTransformPoint(endPoint);

        LR.Points = new Vector2[] { localStartPoint, localEndlocalEndPoint };

        // makes the line slowly appear
        // can mask if the line glitches at the start (it will not visible)
        if (AppearSlowly)
            LR.color = LR.color + Time.deltaTime * new Color(0, 0, 0, LineAppearSpeed);
    }
}
