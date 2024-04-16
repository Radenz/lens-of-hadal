
using Pathfinding;
using UnityEngine;

public static class ObjectExtensions
{
    public static bool TryDowncast<T>(this object obj, out T castObj)
    {
        castObj = default;
        if (typeof(T).IsAssignableFrom(obj.GetType()))
        {
            castObj = (T)obj;
            return true;
        }
        return false;
    }
}

public static class MonoBehaviourExtensions
{
    public static Vector2 RandomWithinRadius(this Transform transform, float radius)
    {
        Vector2 shift = Random.insideUnitCircle * radius;
        return (Vector2)transform.position + shift;
    }

    public static Vector2 RandomOnRadius(this Transform transform, float radius)
    {
        Vector2 shift = Random.insideUnitCircle.normalized * radius;
        return (Vector2)transform.position + shift;
    }
}

public static class AIExtensions
{
    public static bool IsIdle(this IAstarAI ai)
    {
        return ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath);
    }
}
