using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;

public class BladefishAI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _damage = 30;
    [SerializeField]
    private float _attackCooldown;

    [Header("Others")]
    [SerializeField]
    private AutoOrientation _orientation;
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
    [SerializeField, ReadOnly]
    private bool _swinging = false;
    // private bool _hasHit = false;
    [SerializeField]
    private BoxCollider2D _blade;

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

    private bool _aiming = false;

    private void Start()
    {
        _transform = transform;

        _detectionRange.Entered += OnDetectPlayer;
        _aggresionRange.Exited += OnLosePlayer;
        _aggresionRadius = _aggresionRange.GetComponent<CircleCollider2D>().radius;
        _attackHitboxTrigger.Entered += OnAttack;
        _attackHitboxTrigger.Stay += OnAttack;
        _bladeTrigger.Entered += OnSwingHit;
        // _bladeTrigger.Stay += OnSwingHit;

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
        // if (!_swinging || _hasHit) return;
        // _hasHit = true;
        _blade.enabled = false;
        PlayerController.Instance.Damage(_damage);
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

        if (_aiming)
        {
            Vector2 direction = PlayerController.Instance.Position - _transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            _orientation.SetAngle(angle);
        }
    }

    private void OnRoaming()
    {
        if (_ai.reachedDestination || _ai.reachedEndOfPath || !_ai.hasPath)
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
        if (State != BladefishState.Attacking || _swinging) return;
        Stab();
    }

    private async void Stab()
    {
        // _swinging = true;
        // await Awaitable.WaitForSecondsAsync(0.3f);
        // // _hasHit = false;
        // Vector3 angles = _bladeTrigger.transform.localEulerAngles;
        // angles.z = 45;
        // _bladeTrigger.transform.localEulerAngles = angles;
        // angles.z = -45;
        // DisableAI();
        // _blade.enabled = true;
        // await _bladeTrigger.transform.DOLocalRotate(angles, 1f).AsyncWaitForCompletion();
        // await Awaitable.WaitForSecondsAsync(_attackCooldown);
        // AfterSwing();

        _swinging = true;
        _ai.enabled = false;

        _aiming = true;
        await Awaitable.WaitForSecondsAsync(1f);
        // _ai.destination = PlayerController.Instance.Position;
        _aiming = false;
        _blade.enabled = true;
        Vector2 stabDirection = PlayerController.Instance.Position - _transform.position;
        stabDirection = stabDirection.normalized;
        _rigidbody.velocity = stabDirection * 40f;
        _rigidbody.drag = 5f;

        await Awaitable.WaitForSecondsAsync(1f);
        await Awaitable.WaitForSecondsAsync(_attackCooldown);
        AfterSwing();
    }

    private void AfterSwing()
    {
        _rigidbody.drag = 0f;
        if (Vector2.Distance(_transform.position, _player.position) <= _aggresionRadius)
            State = BladefishState.Attacking;
        else
            State = BladefishState.Roaming;
        _ai.enabled = true;
        _swinging = false;
        _blade.enabled = false;
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
