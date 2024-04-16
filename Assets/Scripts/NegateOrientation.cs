using System;
using Pathfinding;
using UnityEngine;

public class NegateOrientation : MonoBehaviour
{
    [SerializeField]
    private Transform _orientation;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        SetFlipped(_orientation.localScale.x < 0);
        Vector3 rotation = _transform.eulerAngles;
        rotation.z = 0;
        _transform.eulerAngles = rotation;
    }

    private void SetFlipped(bool isFlipped)
    {
        float sign = isFlipped ? -1f : 1f;
        Vector3 scale = _transform.localScale;
        scale.x = sign * Mathf.Abs(scale.x);
        _transform.localScale = scale;
    }
}
