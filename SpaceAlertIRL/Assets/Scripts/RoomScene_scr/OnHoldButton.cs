using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class OnHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;

    [SerializeField]
    UnityEvent onHold;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }

    private void FixedUpdate()
    {
        if (pointerDown)
        {
            onHold.Invoke();
        }
    }
}
