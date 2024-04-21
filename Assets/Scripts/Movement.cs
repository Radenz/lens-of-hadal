using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    // TODO: cap velocity to _speed, accelerate based on _acceleration
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private DashProperties _dashProperties;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Vector2 _dashDirection;
    private Vector2 _bounceDirection;

    private bool _isDashing = false;
    private bool _isBouncing = false;

    [SerializeField]
    private bool _canDash = true;
    private bool _shouldDash = false;
    private bool _shouldBounce = false;

    private float _defaultDrag;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _defaultDrag = _rigidbody.drag;
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        _direction = new Vector2(h, v).normalized;

        if (Input.GetKeyDown(_dashProperties.Key))
            _shouldDash = true;
    }

    private void FixedUpdate()
    {
        if (_isDashing || _isBouncing) return;

        if (_shouldBounce)
        {
            _shouldBounce = false;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(_bounceDirection);
            return;
        }

        if (_shouldDash)
            Dash();

        _shouldDash = false;
        _rigidbody.AddForce(_direction * _acceleration);

        if (_rigidbody.velocity.magnitude > _speed)
        {
            _rigidbody.velocity *= _speed / _rigidbody.velocity.magnitude;
        }
    }

    public void Bounce(Vector2 direction, float strength)
    {
        _bounceDirection = direction.normalized * strength;
        _shouldBounce = true;
    }

    private async void Dash()
    {
        _shouldDash = false;
        if (!_canDash) return;

        _canDash = false;
        _isDashing = true;

        // FIXME: get direction from input control
        _dashDirection = 2 * _direction.normalized + _rigidbody.velocity.normalized;
        _dashDirection = _dashDirection.normalized;
        _rigidbody.velocity = _dashDirection * _dashProperties.Speed;
        _rigidbody.drag = 0f;

        await Awaitable.WaitForSecondsAsync(_dashProperties.Duration);
        _isDashing = false;
        _rigidbody.drag = _defaultDrag;

        await Awaitable.WaitForSecondsAsync(_dashProperties.Cooldown);
        _canDash = true;
    }
}

[Serializable]
public struct DashProperties
{
    public float Speed;
    public float Duration;
    public float Cooldown;
    public KeyCode Key;
}
