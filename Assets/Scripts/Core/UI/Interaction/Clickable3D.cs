using UnityEngine;
using UnityEngine.Events;

public class Clickable3D : MonoBehaviour
{
    public UnityEvent OnClick;

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}
