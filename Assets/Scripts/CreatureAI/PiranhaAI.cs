using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// TODO: smoothen bite & run speed
public class PiranhaAI : MonoBehaviour
{
    [SerializeField]
    List<PathFindingBehavior> behaviors = new();

    private readonly Dictionary<PiranhaState, PathFindingBehavior> _behaviorMap = new();

    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private RangeTrigger _detectionRange;
    [SerializeField]
    private RangeTrigger _aggresionRange;
    // TODO: refactor, use IAstarAI interface
    [SerializeField]
    AIPath _ai;

    private PiranhaState _state = PiranhaState.Roaming;
    private bool _aggressive = false;
    private Vector2 _destination;
    private Transform _transform;

    private Vector2 Position => _transform.position;

    [SerializeField]
    private float _idleMinTime = 0.3f;
    [SerializeField]
    private float _idleMaxTime = 2f;
    private TimerSignal _idleTimer;

    private void Start()
    {
        _transform = transform;

        foreach (PathFindingBehavior behavior in behaviors)
        {
            if (behavior is RoamingBehavior)
            {
                _behaviorMap.Add(PiranhaState.Roaming, behavior);
            }

            if (behavior is FollowPlayerBehavior)
            {
                _behaviorMap.Add(PiranhaState.Attacking, behavior);
            }
        }

        _detectionRange.Entered += OnDetectPlayer;
        _aggresionRange.Exited += OnLosePlayer;

        _idleTimer = Timer.Instance.SetTimer(BeginRoam, Random.Range(_idleMinTime, _idleMaxTime));
    }

    private void Update()
    {
        if (_state == PiranhaState.Idle) return;

        if (HasReachedDestination())
            OnDestinationReached();

        if (_aggressive)
        {
            _destination = _behaviorMap[_state].GetDestination();
            _ai.destination = _destination;
        }
    }

    private void OnDestinationReached()
    {
        if (_state == PiranhaState.Roaming)
        {
            _state = PiranhaState.Idle;
            _idleTimer = Timer.Instance.SetTimer(BeginRoam, Random.Range(_idleMinTime, _idleMaxTime));
        }

        if (_state == PiranhaState.Attacking)
        {
            // TODO: select a random point, then try attacking again
            _aggressive = false;
            BeginRoam();
        }
    }

    private void BeginRoam()
    {
        Debug.Log("Being Roaming");
        _state = PiranhaState.Roaming;
        _destination = _behaviorMap[_state].GetDestination();
        _ai.destination = _destination;
    }

    private void OnDetectPlayer()
    {
        _idleTimer?.Cancel();
        _aggressive = true;
        _ai.maxSpeed = 2;
        _state = PiranhaState.Attacking;
        _destination = _behaviorMap[_state].GetDestination();
    }

    private void OnLosePlayer()
    {
        _aggressive = false;
        BeginRoam();
    }

    private bool HasReachedDestination()
    {
        return (Position - _destination).magnitude <= .5f;
    }
}

public enum PiranhaState
{
    Roaming,
    Idle,
    Attacking
}
