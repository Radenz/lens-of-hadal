using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private Vector2 _direction;
    [SerializeField]
    private float _scanAngleRadius;

    private PlayerInputActions _playerInputActions;

    private Camera _camera;
    private Transform _transform;

    // private Transform _objectOnScan;
    private Scannable _objectOnScan;

    [SerializeField]
    private LineRenderer _lineOfSight;
    private List<Scannable> _intersectedScannables = new();

    public bool IsScanning => _direction.magnitude != 0;


    private void Start()
    {
        _camera = Camera.main;
        _transform = transform;
        _playerInputActions = new();
        _playerInputActions.World.Enable();
    }

    private void Update()
    {
        _direction = _playerInputActions.World.Scan.ReadValue<Vector2>();
        Vector3 position = _direction * 2.5f;
        _lineOfSight.SetPosition(1, position);

        if (_objectOnScan != null && _objectOnScan.IsScanned)
        {
            _intersectedScannables.Remove(_objectOnScan);
            StopScanCurrent();
        }
    }

    private void FixedUpdate()
    {
        if (!IsScanning)
        {
            StopScanCurrent();
            return;
        }

        ChooseScannable();
    }

    private void ChooseScannable()
    {
        if (_intersectedScannables.Count == 0) StopScanCurrent();

        if (_intersectedScannables.Count <= 1) return;

        float currentOnScanDistance = _objectOnScan == null ? int.MaxValue : DistanceTo(_objectOnScan.Position);
        Scannable featureToScan = _objectOnScan;

        foreach (Scannable feature in _intersectedScannables)
        {
            if (feature == _objectOnScan) continue;

            if (DistanceTo(feature.Position) < currentOnScanDistance)
                featureToScan = feature;
        }

        if (featureToScan == _objectOnScan) return;

        StopScanCurrent();
        Scan(featureToScan);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Scannable")) return;

        Scannable scannable = other.GetComponent<Scannable>();
        if (scannable.IsScanned) return;

        if (!_intersectedScannables.Contains(scannable) && IsWithinScanRadius(scannable.Position))
        {
            _intersectedScannables.Add(scannable);
            if (_intersectedScannables.Count == 1) Scan(scannable);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Scannable")) return;

        Scannable scannable = other.GetComponent<Scannable>();
        if (scannable.IsScanned) return;

        if (!_intersectedScannables.Contains(scannable) && IsWithinScanRadius(scannable.Position))
        {
            _intersectedScannables.Add(scannable);
            if (_intersectedScannables.Count == 1) Scan(scannable);
        }

        if (_intersectedScannables.Contains(scannable) && !IsWithinScanRadius(scannable.Position))
        {
            _intersectedScannables.Remove(scannable);
            if (_objectOnScan == transform)
                StopScanCurrent();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Scannable")) return;

        Scannable scannable = other.GetComponent<Scannable>();
        _intersectedScannables.Remove(scannable);
    }

    private void Scan(Scannable feature)
    {
        _objectOnScan = feature;
        feature.StartScan();
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
