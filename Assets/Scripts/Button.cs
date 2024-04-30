using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler
{
    public event Action Click;

    public void OnPointerClick(PointerEventData _)
    {
        Click?.Invoke();
    }
}
