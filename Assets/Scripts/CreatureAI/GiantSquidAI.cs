using Pathfinding;
using UnityEngine;

// TODO: adjust ink squirting behavior
public class GiantSquidAI : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private RangeTrigger _detectionRange;
    [SerializeField]
    private RangeTrigger _fleeingRange;

    [SerializeField]
    private GameObject _inkPrefab;

    [SerializeField]
    private float _roamingRadius;
    [SerializeField]
    private float _fleeingRadius;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _fleeingSpeed;

    [SerializeField]
    private float _inkCooldownTime;

    private bool _canReleaseInk = true;

    [SerializeField]
    AIPath _ai;

    private Transform _transform;
    private Transform _player;
    private GiantSquidState _state = GiantSquidState.Roaming;
    private GiantSquidState State
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
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _detectionRange.Entered += OnDetectPlayer;
        _fleeingRange.Exited += OnLosePlayer;

        _ai.maxSpeed = _speed;
        _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
    }

    private void Update()
    {
        switch (State)
        {
            case GiantSquidState.Roaming:
                OnRoaming();
                break;
            case GiantSquidState.Fleeing:
                OnFleeing();
                break;
        }
    }

    private void OnRoaming()
    {
        if (!_ai.hasPath || _ai.IsIdle())
            _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
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

        if (_canReleaseInk) ReleaseInk();
    }

    private async void ReleaseInk()
    {
        _canReleaseInk = false;

        GameObject ink = Instantiate(_inkPrefab, _transform.position, Quaternion.identity);
        Launch launch = ink.GetComponent<Launch>();

        Vector2 inverseDirection = -_ai.velocity.normalized;
        launch.Direction = inverseDirection;

        await Awaitable.WaitForSecondsAsync(_inkCooldownTime);
        _canReleaseInk = true;
    }

    private void OnStateChanged()
    {
        switch (State)
        {
            case GiantSquidState.Roaming:
                _ai.maxSpeed = _speed;
                break;
            case GiantSquidState.Fleeing:
                _ai.maxSpeed = _fleeingSpeed;
                break;
        }
    }

    private void OnDetectPlayer()
    {
        State = GiantSquidState.Fleeing;
    }

    private void OnLosePlayer()
    {
        State = GiantSquidState.Roaming;
    }
}

public enum GiantSquidState
{
    Roaming,
    Fleeing
}
