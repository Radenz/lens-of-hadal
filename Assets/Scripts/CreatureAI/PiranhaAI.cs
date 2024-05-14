using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// TODO: smoothen bite & run speed
public class PiranhaAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _damage = 15;


    [Header("Others")]
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private RangeTrigger _detectionRange;
    [SerializeField]
    private RangeTrigger _aggresionRange;
    [SerializeField]
    private RangeTrigger _attackHitboxTrigger;

    [SerializeField]
    private float _roamingRadius;
    [SerializeField]
    private float _retreatRadius;
    private float _aggresionRadius;


    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _aggresiveSpeed;

    [SerializeField]
    private AIPath _ai;
    private float _lastMaxSpeed;

    private PiranhaState _state = PiranhaState.Roaming;
    private Transform _transform;
    private Transform _player;
    private PiranhaState State
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
        _aggresionRadius = _aggresionRange.GetComponent<CircleCollider2D>().radius;
        _attackHitboxTrigger.Entered += OnAttack;
        _attackHitboxTrigger.Stay += OnAttack;

        // TODO: refactor, use singleton player
        _player = GameObject.FindGameObjectWithTag("Player").transform;

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
            case PiranhaState.Roaming:
                OnRoaming();
                break;
            case PiranhaState.Attacking:
                OnAttacking();
                break;
            case PiranhaState.Retreating:
                OnRetreating();
                break;
        }
    }

    private void OnRoaming()
    {
        if (!_ai.hasPath)
        {
            _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
            return;
        }

        if (_ai.IsIdle())
        {
            _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
        }
    }

    private void OnAttacking()
    {
        _ai.destination = _player.position;
    }

    private void OnAttack()
    {
        if (State != PiranhaState.Attacking) return;
        PlayerController.Instance.Damage(_damage);
        State = PiranhaState.Retreating;
    }

    private void OnRetreating()
    {
        if (_ai.IsIdle())
        {
            if (Vector2.Distance(_transform.position, _player.position) <= _aggresionRadius)
            {
                State = PiranhaState.Attacking;
            }
            else
            {
                State = PiranhaState.Roaming;
            }

        }
    }

    private void OnStateChanged()
    {
        switch (State)
        {
            case PiranhaState.Roaming:
                _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
                _ai.maxSpeed = _speed;
                break;
            case PiranhaState.Attacking:
                _ai.destination = _player.position;
                _ai.maxSpeed = _aggresiveSpeed;
                break;
            case PiranhaState.Retreating:
                _ai.destination = _transform.RandomOnRadius(_retreatRadius);
                break;
        }
    }

    private void OnDetectPlayer()
    {
        State = PiranhaState.Attacking;
    }

    private void OnLosePlayer()
    {
        State = PiranhaState.Roaming;
    }
}

public enum PiranhaState
{
    Roaming,
    Attacking,
    Retreating
}
