using System.Collections.Generic;
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

    [SerializeField]
    private LineRenderer _lineOfSight;
    private List<Transform> _intersectedScannables = new();


    private void Start()
    {
        _camera = Camera.main;
        _transform = transform;
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
        if (_intersectedScannables.Count == 0) StopScanCurrent();

        if (_intersectedScannables.Count <= 1) return;

        float currentOnScanDistance = _objectOnScan == null ? int.MaxValue : DistanceTo(_objectOnScan.position);
        Transform featureToScan = _objectOnScan;

        foreach (Transform feature in _intersectedScannables)
        {
            if (feature == _objectOnScan) continue;

            if (DistanceTo(feature.position) < currentOnScanDistance)
                featureToScan = feature;
        }

        if (featureToScan == _objectOnScan) return;

        StopScanCurrent();
        Scan(featureToScan);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Scannable")) return;

        Transform transform = other.transform;
        if (!_intersectedScannables.Contains(transform) && IsWithinScanRadius(transform.position))
        {
            _intersectedScannables.Add(transform);
            if (_intersectedScannables.Count == 1) Scan(transform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Scannable")) return;

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
        if (!other.CompareTag("Scannable")) return;

        _intersectedScannables.Remove(other.transform);
    }

    private void Scan(Transform feature)
    {
        // TODO: cache scannable
        Debug.Log($"Scanning {feature.gameObject.name}");
        _objectOnScan = feature;
        // _objectOnScanDistance = DistanceTo(feature.position);
        Scannable scannable = feature.GetComponent<Scannable>();
        scannable.StartScan();
    }

    private void StopScanCurrent()
    {
        if (_objectOnScan == null) return;
        Scannable scannable = _objectOnScan.GetComponent<Scannable>();
        scannable.StopScan();
        _intersectedScannables.Remove(_objectOnScan);
        _objectOnScan = null;
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
