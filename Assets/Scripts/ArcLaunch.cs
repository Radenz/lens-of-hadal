using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArcLaunch : MonoBehaviour
{
    public Vector2 Direction;
    public int Power;
    public float Duration;
    public float GravityStrength;

    private Vector2 _modifiedDirection;
    private float _modifiedGravityStrength;
    private Rigidbody2D _rigidbody;
    private Vector2 _initialPosition;
    private Bob _bob;

    private void Awake()
    {
        _bob = GetComponent<Bob>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;
    }

    private void Start()
    {
        // ? Gravity ranges between 0.15 to 0.45
        _modifiedGravityStrength = (Direction.y + 2) * 0.15f;
        _modifiedDirection = Direction;
        _modifiedDirection.y *= _modifiedDirection.y * Mathf.Sign(_modifiedDirection.y);
        _modifiedDirection = _modifiedDirection.normalized;

        _rigidbody.drag = .25f;
        Vector2 impulse = _modifiedDirection * Power;

        _rigidbody.AddForce(impulse, ForceMode2D.Impulse);
        _rigidbody.gravityScale = _modifiedGravityStrength;

        Timer.Instance.SetTimer(SetStasis, Duration);
    }

    private void SetStasis()
    {
        _rigidbody.gravityScale = 0;
        _rigidbody.drag = 5;
        Timer.Instance.SetTimer(() => _bob.StartBobbing(), 0.3f);
    }

    [Button]
    private void Reset()
    {
        transform.position = _initialPosition;
        Start();
    }
}
