using System;
using Pathfinding;
using UnityEngine;

public class AutoOrientation : MonoBehaviour
{
    [SerializeField]
    private AIPath _ai;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _ai.velocity;
        float angle = Vector2.SignedAngle(Vector2.right, velocity);

        if (angle == 0) return;

        SetAngle(angle);
    }

    public void SetAngle(float angle)
    {
        if (Mathf.Abs(angle) > 90f)
        {
            angle = -Mathf.Sign(angle) * (180 - Mathf.Abs(angle));
            SetFlipped(true);
        }
        else
        {
            SetFlipped(false);
        }

        Vector3 rotation = _transform.localEulerAngles;
        rotation.z = angle;
        _transform.localEulerAngles = rotation;
    }

    private void SetFlipped(bool isFlipped)
    {
        float sign = isFlipped ? -1f : 1f;
        Vector3 scale = _transform.localScale;
        scale.x = sign * Mathf.Abs(scale.x);
        _transform.localScale = scale;
    }
}
