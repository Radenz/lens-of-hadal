using NaughtyAttributes;
using Pathfinding;
using UnityEngine;

public class ElectricEelAI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _damage = 6;

    [Header("Others")]
    [SerializeField]
    private GameObject _zapPrefab;
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

    private bool _canZap = true;

    private void Start()
    {
        _transform = transform;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _detectionRange.Entered += OnDetectPlayer;
        _fleeingRange.Exited += OnLosePlayer;

        _ai.maxSpeed = _speed;
        _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
        _ai.SearchPath();

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
        _canZap = false;

        // TODO: play zap sfx
        PlayerController.Instance.Damage(_damage);
        PlayerController.Instance.Shock(_zapShockDuration);
        SpawnZap(_transform.position, PlayerController.Instance.Position);

        Timer.Instance.SetTimer(() => _canZap = true, _zapCooldownTime);
    }

    private void SpawnZap(Vector2 from, Vector2 to)
    {
        float length = Vector2.Distance(from, to);
        Vector2 center = (from + to) / 2;
        Vector2 direction = to - from;

        float angle = Vector2.SignedAngle(Vector2.up, direction);

        GameObject zap = Instantiate(_zapPrefab, center, Quaternion.identity);
        zap.transform.localScale = zap.transform.localScale.With(y: length);
        zap.transform.localEulerAngles = zap.transform.localEulerAngles.With(z: angle);
    }

    private void OnRoaming()
    {
        if (!_ai.hasPath)
        {
            _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
            return;
        }

        if (!CanReachDestination() || _ai.IsIdle())
        {
            _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
        }
    }

    private bool CanReachDestination()
    {
        GraphNode node1 = AstarPath.active.GetNearest(_ai.destination, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(_transform.position, NNConstraint.Default).node;
        return PathUtilities.IsPathPossible(node1, node2);
    }

    [Button]
    private void CheckReachability()
    {
        Debug.Log("Can reach? " + CanReachDestination());
    }

    private void OnFleeing(bool isForced = false)
    {
        if (_canZap) Zap();

        if (_ai.IsIdle() || !CanReachDestination() || isForced)
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
                break;
            case ElectricEelState.Fleeing:
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
