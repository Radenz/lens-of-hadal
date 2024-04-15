
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoamingBehavior : PathFindingBehavior
{
    // ! Only supports box & circle collider
    [SerializeField]
    private List<Collider2D> _roamingArea = new();
    private List<float> _weights;

    private void Start()
    {
        _weights = _roamingArea.Select(area => CalculateArea(area)).ToList();

        float totalWeight = 0;
        foreach (float weight in _weights)
        {
            totalWeight += weight;
        }

        _weights = _weights.Select(weight => weight / totalWeight).ToList();
    }

    private static float CalculateArea(Collider2D bound)
    {
        if (bound.TryDowncast(out BoxCollider2D boxBound))
        {
            Vector2 size = boxBound.size;
            return size.x * size.y;
        }

        if (bound.TryDowncast(out CircleCollider2D circleBound))
        {
            float radius = circleBound.radius;
            return Mathf.PI * radius * radius;
        }

        return 0;
    }

    public override Vector2 GetDestination()
    {
        // FIXME: use random in unit circle to find a point
        //        then add to current position
        // @see https://arongranberg.com/astar/docs/wander.html
        float number = Random.value;
        Collider2D area = null;

        float cumulativeWeight = 0f;
        for (int i = 0; i < _weights.Count; i++)
        {
            float weight = _weights[i];
            cumulativeWeight += weight;
            if (number <= cumulativeWeight)
            {
                area = _roamingArea[i];
                break;
            }
        }

        return PickPoint(area);
    }

    private Vector2 PickPoint(Collider2D bound)
    {
        if (bound.TryDowncast(out BoxCollider2D boxBound))
        {
            Vector2 min = boxBound.bounds.min;
            Vector2 size = boxBound.size;

            float xShift = Random.Range(0, size.x);
            float yShift = Random.Range(0, size.y);

            return new(min.x + xShift, min.y + yShift);
        }

        if (bound.TryDowncast(out CircleCollider2D circleBound))
        {
            Vector2 point = Random.insideUnitCircle * circleBound.radius;
            Vector2 center = circleBound.bounds.center;
            return center + point;
        }

        return default;
    }
}
