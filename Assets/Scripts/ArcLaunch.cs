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

    [SerializeField]
    private float _dropGravity = 0f;

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
        _rigidbody.gravityScale = _dropGravity;
        _rigidbody.drag = 5;
    }

    [Button]
    private void Reset()
    {
        transform.position = _initialPosition;
        Start();
    }
}
