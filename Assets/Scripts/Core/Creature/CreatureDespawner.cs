using UnityEngine;

public class CreatureDespawner : MonoBehaviour
{
    public Creature Creature;

    private Transform _transform;
    public Vector3 Position => _transform.position;

    private void Start()
    {
        _transform = transform;
        CreatureManager.Instance.Register(this);
    }

    private void Update()
    {
        if (DistanceToPlayer() > CreatureManager.Instance.DespawnRadius)
            Despawn();
    }

    private float DistanceToPlayer()
    {
        return Vector2.Distance(PlayerController.Instance.Position, _transform.position);
    }

    private void Despawn()
    {
        CreatureManager.Instance.Unregister(this);
        Destroy(gameObject);
    }
}
