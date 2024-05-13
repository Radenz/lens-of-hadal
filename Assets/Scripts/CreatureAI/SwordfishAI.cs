using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// TODO: smoothen bite & run speed
public class SwordfishAI : MonoBehaviour
{
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

    private SwordfishState _state = SwordfishState.Roaming;
    private Transform _transform;
    private Transform _player;
    private SwordfishState State
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
            case SwordfishState.Roaming:
                OnRoaming();
                break;
            case SwordfishState.Attacking:
                OnAttacking();
                break;
            case SwordfishState.Retreating:
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
        if (State != SwordfishState.Attacking) return;
        PlayerController.Instance.Damage(15);
        State = SwordfishState.Retreating;
    }

    private void OnRetreating()
    {
        if (_ai.IsIdle())
        {
            if (Vector2.Distance(_transform.position, _player.position) <= _aggresionRadius)
            {
                State = SwordfishState.Attacking;
            }
            else
            {
                State = SwordfishState.Roaming;
            }

        }
    }

    private void OnStateChanged()
    {
        switch (State)
        {
            case SwordfishState.Roaming:
                _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
                _ai.maxSpeed = _speed;
                break;
            case SwordfishState.Attacking:
                _ai.destination = _player.position;
                _ai.maxSpeed = _aggresiveSpeed;
                break;
            case SwordfishState.Retreating:
                _ai.destination = _transform.RandomOnRadius(_retreatRadius);
                break;
        }
    }

    private void OnDetectPlayer()
    {
        State = SwordfishState.Attacking;
    }

    private void OnLosePlayer()
    {
        State = SwordfishState.Roaming;
    }
}

public enum SwordfishState
{
    Roaming,
    Attacking,
    Retreating
}
