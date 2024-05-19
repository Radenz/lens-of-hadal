using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _creaturePrefab;

    [SerializeField]
    private BoxCollider2D _boundsCollider;
    [SerializeField]
    private float _spawnAttemptInterval;

    private TimerSignal _signal;
    private Vector2 _spawnBoundMin;
    private Vector2 _spawnBoundMax;

    private void Start()
    {
        _signal = Timer.Instance.SetTimer(AttemptPeriodicSpawn, _spawnAttemptInterval);
        _spawnBoundMin = _boundsCollider.bounds.min;
        _spawnBoundMax = _boundsCollider.bounds.max;
    }

    private void AttemptPeriodicSpawn()
    {
        if (CreatureManager.Instance.CreatureCount < CreatureManager.Instance.MaxCreatures)
            Spawn();
        _signal = Timer.Instance.SetTimer(AttemptPeriodicSpawn, _spawnAttemptInterval);
    }

    private void Spawn()
    {
        Instantiate(_creaturePrefab, PickSpawnPoint(), Quaternion.identity);
    }

    private Vector2 PickSpawnPoint()
    {
        float x = Random.Range(_spawnBoundMin.x, _spawnBoundMax.x);
        float y = Random.Range(_spawnBoundMin.y, _spawnBoundMax.y);
        return new Vector2(x, y);
    }

    private void OnDestroy()
    {
        _signal.Cancel();
    }
}
