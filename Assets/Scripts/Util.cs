
using System.Linq;
using Pathfinding;
using TMPro;
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

public static class TransformExtensions
{
    public static Vector3 With(this Vector3 vec, float? x, float? y, float? z)
    {
        vec.x = x ?? vec.x;
        vec.y = y ?? vec.z;
        vec.z = z ?? vec.z;
        return vec;
    }

    public static Vector3 Add(this Vector3 vec, float x = 0, float y = 0, float z = 0)
    {
        vec.x += x;
        vec.y += y;
        vec.z += z;
        return vec;
    }
}

public static class AIExtensions
{
    public static bool IsIdle(this IAstarAI ai)
    {
        return !ai.pathPending && (ai.reachedDestination || ai.reachedEndOfPath || !ai.hasPath);
    }
}

public static class TMProExtensions
{
    public static float TotalHeight(this TextMeshProUGUI textMesh)
    {
        TMP_LineInfo[] lines = textMesh.GetTextInfo(textMesh.text).lineInfo;
        Debug.Log(textMesh.text);
        Debug.Log(lines.Length);
        float height = 0f;

        foreach (TMP_LineInfo line in lines)
        {
            height += line.lineHeight;
        }

        return height;
    }

    public static void FitHeight(this TextMeshProUGUI textMesh)
    {
        textMesh.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textMesh.TotalHeight());
    }
}
