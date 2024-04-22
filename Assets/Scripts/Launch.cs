using DG.Tweening;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public Vector2 Direction;
    public int Power;
    public float Duration;

    private void Start()
    {
        Vector2 shift = Direction * Power;
        Transform transform_ = transform;
        Vector2 endPosition = (Vector2)transform_.position + shift;

        transform_.DOMove(endPosition, Duration);
    }
}
