using Pathfinding;
using UnityEngine;

public class ElectricEelAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _damage = 6;

    [Header("Others")]
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private RangeTrigger _detectionRange;
    [SerializeField]
    private RangeTrigger _fleeingRange;

    [SerializeField]
    private float _roamingRadius;
    [SerializeField]
    private float _fleeingRadius;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _fleeingSpeed;

    [SerializeField]
    private float _zapCooldownTime;
    [SerializeField]
    private float _zapShockDuration;

    [SerializeField]
    AIPath _ai;
    private float _lastMaxSpeed;

    private Transform _transform;
    private Transform _player;
    private ElectricEelState _state = ElectricEelState.Roaming;
    private ElectricEelState State
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChanged();
        }
    }

    private TimerSignal _zapTimer;

    private void Start()
    {
        _transform = transform;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _detectionRange.Entered += OnDetectPlayer;
        _fleeingRange.Exited += OnLosePlayer;

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
            case ElectricEelState.Roaming:
                OnRoaming();
                break;
            case ElectricEelState.Fleeing:
                OnFleeing();
                break;
        }
    }

    private void Zap()
    {
        // ? Extra guard just in case flee state change missed
        if (State == ElectricEelState.Roaming) return;

        // TODO: show zap vfx, play zap sfx
        PlayerController.Instance.Damage(_damage);
        PlayerController.Instance.Shock(_zapShockDuration);
        _zapTimer = Timer.Instance.SetTimer(Zap, _zapCooldownTime);
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

    private void OnFleeing(bool isForced = false)
    {
        if (_ai.IsIdle() || isForced)
        {
            Vector2 relativePosition = _transform.position - _player.position;
            Vector2 fleePosition = _transform.RandomOnRadius(_fleeingRadius) - (Vector2)_player.position;

            float angle = Vector2.Angle(relativePosition, fleePosition);
            if (angle > 90)
            {
                fleePosition = -fleePosition;
            }

            _ai.destination = (Vector2)_transform.position + fleePosition;
        }
    }

    private void OnStateChanged()
    {
        switch (State)
        {
            case ElectricEelState.Roaming:
                _ai.maxSpeed = _speed;
                _zapTimer?.Cancel();
                break;
            case ElectricEelState.Fleeing:
                _zapTimer = Timer.Instance.SetTimer(Zap, _zapCooldownTime);
                _ai.maxSpeed = _fleeingSpeed;
                OnFleeing(isForced: true);
                break;
        }
    }

    private void OnDetectPlayer()
    {
        State = ElectricEelState.Fleeing;
    }

    private void OnLosePlayer()
    {
        State = ElectricEelState.Roaming;
    }
}

public enum ElectricEelState
{
    Roaming,
    Fleeing
}
