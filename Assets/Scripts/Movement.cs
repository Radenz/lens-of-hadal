using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public static Movement Instance;

    // TODO: cap velocity to _speed, accelerate based on _acceleration
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private DashProperties _dashProperties;

    private PlayerInputActions _playerInputActions;

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

    public event Action Dashing;
    public event Action DashCooldownFinished;

    private float _defaultDrag;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _defaultDrag = _rigidbody.drag;
        _playerInputActions = new();

        _playerInputActions.World.Enable();
        _playerInputActions.World.Dash.performed += _ => _shouldDash = true;
    }

    private void FixedUpdate()
    {
        _direction = _playerInputActions.World.Move.ReadValue<Vector2>();

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
    }

    public void Bounce(Vector2 direction, float strength)
    {
        _bounceDirection = direction.normalized * strength;
        _shouldBounce = true;
    }

    public void Shock(float duration)
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        Timer.Instance.SetTimer(() => _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation, duration);
    }

    private async void Dash()
    {
        _shouldDash = false;
        if (!_canDash) return;

        _canDash = false;
        _isDashing = true;

        Dashing?.Invoke();

        _rigidbody.velocity = _direction * _dashProperties.Speed;
        _rigidbody.drag = 0f;

        await Awaitable.WaitForSecondsAsync(_dashProperties.Duration);
        _isDashing = false;
        _rigidbody.drag = _defaultDrag;

        await Awaitable.WaitForSecondsAsync(_dashProperties.Cooldown);
        DashCooldownFinished?.Invoke();
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
