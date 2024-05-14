using System;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

// TODO: smoothen bite & run speed
public class BladefishAI : MonoBehaviour
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
    private RangeTrigger _bladeTrigger;

    [SerializeField]
    private float _roamingRadius;
    [SerializeField]
    private float _retreatRadius;
    private float _aggresionRadius;
    private bool _swinging = false;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _aggresiveSpeed;

    [SerializeField]
    private AIPath _ai;
    private float _lastMaxSpeed;

    private BladefishState _state = BladefishState.Roaming;
    private Transform _transform;
    private Transform _player;
    private BladefishState State
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
        _bladeTrigger.Entered += OnSwingHit;

        // TODO: refactor, use singleton player
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _ai.maxSpeed = _speed;
        _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
        EventManager.Instance.CreaturesDisabled += DisableAI;
        EventManager.Instance.CreaturesEnabled += EnableAI;
    }

    private void OnSwingHit()
    {
        if (!_swinging) return;
        PlayerController.Instance.Damage(30);
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
            case BladefishState.Roaming:
                OnRoaming();
                break;
            case BladefishState.Attacking:
                OnAttacking();
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
        if (_swinging) return;
        _ai.destination = _player.position;
    }

    private void OnAttack()
    {
        if (State != BladefishState.Attacking) return;
        Swing();
    }

    private void Swing()
    {
        _swinging = false;
        Vector3 angles = _bladeTrigger.transform.localEulerAngles;
        angles.z = 45;
        _bladeTrigger.transform.localEulerAngles = angles;
        _swinging = true;
        angles.z = -45;
        _bladeTrigger.transform.DOLocalRotate(angles, 1f).OnComplete(AfterSwing);
    }

    private void AfterSwing()
    {
        _swinging = false;
        if (Vector2.Distance(_transform.position, _player.position) <= _aggresionRadius)
            State = BladefishState.Attacking;
        else
            State = BladefishState.Roaming;
    }

    private void OnStateChanged()
    {
        switch (State)
        {
            case BladefishState.Roaming:
                _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
                _ai.maxSpeed = _speed;
                break;
            case BladefishState.Attacking:
                _ai.destination = _player.position;
                _ai.maxSpeed = _aggresiveSpeed;
                break;
        }
    }

    private void OnDetectPlayer()
    {
        State = BladefishState.Attacking;
    }

    private void OnLosePlayer()
    {
        State = BladefishState.Roaming;
    }
}

public enum BladefishState
{
    Roaming,
    Attacking
}
