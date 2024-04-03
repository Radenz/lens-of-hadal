using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private Vector2 _direction;
    [SerializeField]
    private float _scanAngleRadius;

    private Camera _camera;
    private Transform _transform;

    private Transform _objectOnScan;

    private LineRenderer _lineOfSight;
    private List<Transform> _intersectedScannables = new();


    private void Start()
    {
        _camera = Camera.main;
        _transform = transform;
        _lineOfSight = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 scannerPosition = _transform.position;
        _direction = (mousePosition - scannerPosition).normalized;

        Vector3 position = _direction * 2.5f;
        _lineOfSight.SetPosition(1, position);
    }

    private void FixedUpdate()
    {
        ChooseScannable();
    }

    private void ChooseScannable()
    {
        if (_intersectedScannables.Count == 1) return;

        Transform featureToScan = null;
        float currentOnScanDistance = _objectOnScan == null ? int.MaxValue : DistanceTo(_objectOnScan.position);

        foreach (Transform feature in _intersectedScannables)
        {
            if (feature == _objectOnScan) continue;

            if (DistanceTo(feature.position) < currentOnScanDistance)
                featureToScan = feature;
        }

        if (featureToScan != null)
        {
            Scan(featureToScan);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform transform = other.transform;
        if (!_intersectedScannables.Contains(transform) && IsWithinScanRadius(transform.position))
        {
            _intersectedScannables.Add(transform);
            if (_intersectedScannables.Count == 1) Scan(transform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Transform transform = other.transform;
        if (!_intersectedScannables.Contains(transform) && IsWithinScanRadius(transform.position))
        {
            _intersectedScannables.Add(transform);
            if (_intersectedScannables.Count == 1) Scan(transform);
        }

        if (_intersectedScannables.Contains(transform) && !IsWithinScanRadius(transform.position))
        {
            _intersectedScannables.Remove(transform);
            if (_objectOnScan == transform)
                StopScanCurrent();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _intersectedScannables.Remove(other.transform);
    }

    private void Scan(Transform feature)
    {
        // TODO: cache scannable
        if (_objectOnScan != null)
        {
            Scannable currentScannable = _objectOnScan.GetComponent<Scannable>();
            currentScannable.StopScan();
        }

        Debug.Log($"Scanning {feature.gameObject.name}");
        _objectOnScan = feature;
        // _objectOnScanDistance = DistanceTo(feature.position);
        Scannable scannable = feature.GetComponent<Scannable>();
        scannable.StartScan();
    }

    private void StopScanCurrent()
    {
        Scannable scannable = _objectOnScan.GetComponent<Scannable>();
        scannable.StopScan();
        _objectOnScan = null;
        _intersectedScannables.Remove(_objectOnScan);
    }

    private float DistanceTo(Vector3 position)
    {
        return Vector3.Cross(_direction, position - _transform.position).magnitude;
        // return Vector3.Distance(_transform.position, position);
    }

    private bool IsWithinScanRadius(Vector3 position)
    {
        Vector2 relativePosition = position - _transform.position;
        float angle = Vector2.Angle(_direction, relativePosition);

        return angle < _scanAngleRadius;
    }
}