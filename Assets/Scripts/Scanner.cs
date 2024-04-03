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
    private float _objectOnScanDistance;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform transform = other.transform;
        if (!IsWithinScanRadius(transform.position)) return;

        if (_objectOnScan == null)
        {
            Debug.Log($"Scanning {transform.gameObject.name}");
            _objectOnScan = transform;
            _objectOnScanDistance = DistanceTo(_objectOnScan.position);
            return;
        }

        float distance = DistanceTo(transform.position);
        if (distance < _objectOnScanDistance)
        {
            Debug.Log($"Scanning {transform.gameObject.name}");
            _objectOnScan = transform;
            _objectOnScanDistance = distance;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Transform transform = other.transform;
        if (!IsWithinScanRadius(transform.position)) return;

        if (transform == _objectOnScan) return;

        float distance = DistanceTo(transform.position);
        if (distance < _objectOnScanDistance)
        {
            Debug.Log($"Scanning {transform.gameObject.name}");
            _objectOnScan = transform;
            _objectOnScanDistance = distance;
        }
    }

    private float DistanceTo(Vector3 position)
    {
        // return Vector3.Cross(_direction, position - _transform.position).magnitude;
        return Vector3.Distance(_transform.position, position);
    }

    private bool IsWithinScanRadius(Vector3 position)
    {
        Vector2 relativePosition = position - _transform.position;
        float angle = Vector2.Angle(_direction, relativePosition);

        return angle < _scanAngleRadius;
    }
}