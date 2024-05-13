using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private Vector2 _direction;
    private float _range = 2.5f;

    [SerializeField]
    private CircleCollider2D _rangeCollider;

    [SerializeField]
    private float _scanAngleRadius;

    private PlayerInputActions _playerInputActions;

    private Transform _transform;
    [SerializeField]
    private AutoFlip _playerFlip;
    [SerializeField]
    private Transform _lightTransform;
    private GameObject _light;

    private Scannable _objectOnScan;
    private readonly List<Scannable> _intersectedScannables = new();

    public bool IsScanning => _direction.magnitude != 0;


    private void Start()
    {
        _transform = transform;
        _light = _lightTransform.gameObject;
        _playerInputActions = new();
        _playerInputActions.World.Enable();

        EventManager.Instance.ScannerEquipped += OnScannerEquipped;
        EventManager.Instance.ScannerUnequipped += OnScannerUnequipped;
    }

    private void Update()
    {
        _direction = _playerInputActions.World.Scan.ReadValue<Vector2>();
        if (!IsScanning)
        {
            _light.SetActive(false);
            return;
        }

        _light.SetActive(true);
        float angle = Vector2.SignedAngle(Vector2.right, _direction);

        Vector3 lightAngles = _lightTransform.eulerAngles;
        lightAngles.z = -90 + angle;
        _lightTransform.eulerAngles = lightAngles;
        _playerFlip.SetAngle(angle);


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
        else
        {
            ChooseScannable();
        }
    }

    private void OnScannerEquipped(int level)
    {
        if (level == 2)
        {
            SetRange(2.7f);
            return;
        }

        if (level == 3)
            SetRange(3f);
    }

    private void OnScannerUnequipped()
    {
        SetRange(2.5f);
    }

    private void SetRange(float range)
    {
        _rangeCollider.radius = range;
    }

    private void ChooseScannable()
    {
        if (_intersectedScannables.Count == 0) StopScanCurrent();

        if (_intersectedScannables.Count <= 1 && _objectOnScan != null) return;

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
            // if (_intersectedScannables.Count == 1) Scan(scannable);
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
            // if (_intersectedScannables.Count == 1) Scan(scannable);
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
