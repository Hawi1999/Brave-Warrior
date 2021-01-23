using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Control : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool KeyOne;
    public void OnPointerDown(PointerEventData eventData)
    {
        KeyOne = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        KeyOne = false;
    }

    private void OnDestroy()
    {
        KeyOne = false;
    }
}
