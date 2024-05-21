using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    private Transform _transform;

    [SerializeField]
    private GameObject _creaturePrefab;

    [SerializeField]
    private BoxCollider2D _boundsCollider;
    [SerializeField]
    private float _spawnAttemptIntervalBase;

    private TimerSignal _signal;

    private float _xExtent;
    private float _yExtent;

    private void Start()
    {
        if (_spawnAttemptIntervalBase == 0) _spawnAttemptIntervalBase = 2;

        _transform = transform;
        _signal = Timer.Instance.SetTimer(AttemptPeriodicSpawn, SpawnAttemptInterval());
        Vector3 extent = _boundsCollider.bounds.extents;
        _xExtent = extent.x;
        _yExtent = extent.y;
    }

    private float SpawnAttemptInterval()
    {
        return _spawnAttemptIntervalBase * 10 /
            (10 + CreatureManager.Instance.MaxCreatures);
    }

    private void AttemptPeriodicSpawn()
    {
        if (CreatureManager.Instance.CreatureCount < CreatureManager.Instance.MaxCreatures)
        {
            Vector3 spawnPoint = PickSpawnPoint();
            float distanceToPlayer = DistanceToPlayer(spawnPoint);


            if (distanceToPlayer >= CreatureManager.Instance.SpawnRadius
                && distanceToPlayer < CreatureManager.Instance.DespawnRadius
            )
                Spawn(spawnPoint);
        }

        _signal = Timer.Instance.SetTimer(AttemptPeriodicSpawn, SpawnAttemptInterval());
    }

    private float DistanceToPlayer(Vector3 position)
    {
        return Vector2.Distance(position, PlayerController.Instance.Position);
    }

    private void Spawn(Vector3 position)
    {
        Instantiate(_creaturePrefab, position, Quaternion.identity);
    }

    private Vector3 PickSpawnPoint()
    {
        float x = Random.Range(-_xExtent, _xExtent);
        float y = Random.Range(-_yExtent, _yExtent);
        Vector3 point = _transform.TransformPoint(new Vector2(x, y));
        return point;
    }

    private void OnDestroy()
    {
        _signal.Cancel();
    }
}
