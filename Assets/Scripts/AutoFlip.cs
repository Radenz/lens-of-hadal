using UnityEngine;

public class AutoFlip : MonoBehaviour
{
    [SerializeField]
    private Scanner _scanner;

    [SerializeField]
    private float _maxTiltAngle = 60;
    [SerializeField]
    private float _smoothingFactor = 0.5f;

    public float Angle { get; private set; }

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rigidbody.velocity;
        float angle = Vector2.SignedAngle(Vector2.right, velocity);
        if (angle == 0) return;

        if (Mathf.Sign(angle) == Mathf.Sign(Angle) || Mathf.Abs(angle) < 90f)
        {
            Angle = Mathf.Lerp(Angle, angle, _smoothingFactor);
        }
        else
        {
            float wrappedAngle = angle - 360 * Mathf.Sign(angle);
            Angle = Mathf.Lerp(Angle, wrappedAngle, _smoothingFactor);
        }

        if (_scanner.IsScanning) return;

        SetAngle(Angle);
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

        if (Mathf.Abs(angle) > _maxTiltAngle)
        {
            angle = Mathf.Sign(angle) * _maxTiltAngle;
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
