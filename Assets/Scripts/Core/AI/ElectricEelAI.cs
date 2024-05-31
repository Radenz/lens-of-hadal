using System.Threading.Tasks;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;

public class ElectricEelAI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _damage = 6;
    [SerializeField]
    private float _zapDelay = 1f;
    [SerializeField]
    private float _zapEscapeRadius = 3.5f;

    [Header("Others")]
    [SerializeField]
    private GameObject _zapPrefab;
    [SerializeField]
    private GameObject _static;
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private RangeTrigger _detectionRange;
    [SerializeField]
    private RangeTrigger _fleeingRange;
    [SerializeField]
    private RangeTrigger _zapEscapeRange;


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
    private AudioClip _zapSFX;

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
    private bool _isTryingToZap = false;
    private bool _shouldZap = false;
    private bool _disabled = false;

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
        _disabled = true;
    }

    private void EnableAI()
    {
        _ai.maxSpeed = _lastMaxSpeed;
        _disabled = false;
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

        if (_shouldZap && _canZap)
            TryZap();
    }

    private async void TryZap()
    {
        if (_isTryingToZap || _disabled)
            return;
        _isTryingToZap = true;

        _static.SetActive(true);
        await Awaitable.WaitForSecondsAsync(_zapDelay);
        _static.SetActive(false);

        _isTryingToZap = false;
        if (DistanceToPlayer() > _zapEscapeRadius) return;
        Zap();
    }

    private float DistanceToPlayer()
    {
        return Vector2.Distance(_player.position, _transform.position);
    }

    private void Zap()
    {
        if (!_canZap || _disabled) return;
        _canZap = false;

        AudioManager.Instance.PlaySFX(_zapSFX);
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
        if (_ai.reachedDestination || _ai.reachedEndOfPath || !_ai.hasPath)
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

    private void OnFleeing(bool isForced = false)
    {
        if (_canZap) TryZap();

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
        _shouldZap = true;
    }

    private void OnLosePlayer()
    {
        State = ElectricEelState.Roaming;
        _shouldZap = false;
    }
}

public enum ElectricEelState
{
    Roaming,
    Fleeing
}
