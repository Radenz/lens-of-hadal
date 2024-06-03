using Pathfinding;
using UnityEngine;

public class PufferfishAI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _damage = 10;

    [Header("Others")]
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private Transform _spriteTransform;

    [SerializeField]
    private RangeTrigger _inflatedHitboxTrigger;

    [SerializeField]
    private RangeTrigger _detectionRange;
    [SerializeField]
    private RangeTrigger _aggresionRange;

    [SerializeField]
    private float _roamingRadius;
    [SerializeField]
    private float _inflatedRoamingRadius;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _inflatedSpeed;
    [SerializeField]
    private float _bounceStrength;

    [SerializeField]
    AIPath _ai;
    private float _lastMaxSpeed;

    private Transform _transform;
    private Transform _playerTransform;
    private PufferfishState _state = PufferfishState.Roaming;
    private PufferfishState State
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChanged();
        }
    }

    private void Start()
    {
        _transform = transform;

        _detectionRange.Entered += OnDetectPlayer;
        _aggresionRange.Exited += OnLosePlayer;
        _inflatedHitboxTrigger.Entered += BouncePlayer;
        _playerTransform = PlayerController.Instance.GetComponent<Transform>();

        _ai.maxSpeed = _speed;
        _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
        EventManager.Instance.CreaturesDisabled += DisableAI;
        EventManager.Instance.CreaturesEnabled += EnableAI;
    }

    private void DisableAI()
    {
        _lastMaxSpeed = _ai.maxSpeed;
        _ai.maxSpeed = 0;
    }

    private void EnableAI()
    {
        _ai.maxSpeed = _lastMaxSpeed;
    }

    private void Update()
    {
        switch (State)
        {
            case PufferfishState.Roaming:
                OnRoaming();
                break;
            case PufferfishState.Inflated:
                OnInflated();
                break;
        }
    }

    private void OnRoaming()
    {
        if (_ai.reachedDestination || _ai.reachedEndOfPath || !_ai.hasPath)
        {
            _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
        }
    }

    private void OnInflated()
    {
        if (_ai.reachedDestination || _ai.reachedEndOfPath || !_ai.hasPath)
        {
            _ai.destination = _transform.RandomWithinRadius(_inflatedRoamingRadius);
        }
    }

    private void BouncePlayer()
    {
        if (State != PufferfishState.Inflated) return;

        Vector2 direction = _playerTransform.position - _transform.position;
        PlayerController.Instance.Bounce(direction, _bounceStrength);
        PlayerController.Instance.Damage(_damage);

        State = PufferfishState.Roaming;
    }

    // TODO: switch sprite to inflated, activate collider & damage source
    private void OnStateChanged()
    {
        switch (State)
        {
            case PufferfishState.Roaming:
                _ai.maxSpeed = _speed;
                break;
            case PufferfishState.Inflated:
                _ai.maxSpeed = _inflatedSpeed;
                break;
        }
    }

    private void OnDetectPlayer()
    {
        State = PufferfishState.Inflated;
        _spriteTransform.localScale = new(1.2f, 1.2f, 1.2f);
    }

    private void OnLosePlayer()
    {
        State = PufferfishState.Roaming;
        _spriteTransform.localScale = Vector3.one;
    }
}

public enum PufferfishState
{
    Roaming,
    Inflated
}
