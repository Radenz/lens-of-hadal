using UnityEngine;

public class AutoFlip : MonoBehaviour
{
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
