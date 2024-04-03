using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private DashProperties _dashProperties;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Vector2 _dashDirection;

    private bool _shouldDash = false;
    private bool _isDashing = false;
    private float _dashTime = 0;
    private float _dashCooldownTime = 0;


    void Start()
    {
        _dashCooldownTime = _dashProperties.Cooldown;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        _direction = new Vector2(h, v).normalized;

        if (Input.GetKeyDown(_dashProperties.Key))
        {
            if (_dashCooldownTime < _dashProperties.Cooldown)
            {
                return;
            }

            if (!_isDashing)
            {
                _dashCooldownTime = 0;
                _shouldDash = true;
            }
        }

        _dashCooldownTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_shouldDash)
        {
            _shouldDash = false;
            _isDashing = true;
            _dashTime = 0;
            _dashDirection = 2 * _direction.normalized + _rigidbody.velocity.normalized;
            _dashDirection = _dashDirection.normalized;
            return;
        }

        if (_isDashing)
        {
            _rigidbody.velocity = _dashDirection * _dashProperties.Speed;
            _dashTime += Time.deltaTime;

            if (_dashTime >= _dashProperties.Duration)
            {
                _isDashing = false;
            }
            return;
        }

        _shouldDash = false;
        _rigidbody.AddForce(_direction * _speed);
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