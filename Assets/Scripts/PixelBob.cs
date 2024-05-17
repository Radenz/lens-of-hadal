using System;
using DG.Tweening;
using UnityEngine;

public class PixelBob : MonoBehaviour
{
    private Transform _transform;

    [SerializeField]
    private float _offset;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private Axis _axis;
    [SerializeField]
    private int _bobCount;
    [SerializeField]
    private bool _startImmediately = false;
    private bool _isOffset = false;

    private void Start()
    {
        _transform = transform;
        if (_startImmediately) StartBobbing();
    }

    public void StartBobbing()
    {
        Timer.Instance.SetTimer(ApplyBob, _duration);
    }

    private void ApplyBob()
    {
        float shift = _isOffset ? -_offset : _offset;
        Vector3 newPosition = _transform.position.Add(
            x: _axis == Axis.X ? shift : 0,
            y: _axis == Axis.Y ? shift : 0,
            z: _axis == Axis.Z ? shift : 0
        );
        _isOffset = !_isOffset;
        _transform.position = newPosition;
        Timer.Instance.SetTimer(ApplyBob, _duration);
    }
}

[Serializable]
public enum Axis
{
    X,
    Y,
    Z
}
