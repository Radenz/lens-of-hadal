using System;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class ModuleGrid : MonoBehaviour
{
    private RectTransform _transform;
    private RectTransform _canvasTransform;
    private Matrix4x4 _mvpMatrix;

    [SerializeField]
    private Vector2Int _gridSize;

    private Module[,] _modules;

    [SerializeField]
    private Vector2 _tileSize = new(160, 160);

    public event Action<Module> Place;

    Rect _rect => _transform.rect;
    float _xStart => _rect.xMin + _transform.localPosition.x;
    float _yStart => _rect.yMin + _transform.localPosition.y;
    float _xEnd => _rect.xMax + _transform.localPosition.x;
    float _yEnd => _rect.yMax + _transform.localPosition.y;

    private void Awake()
    {
        _transform = (RectTransform)transform;
        Canvas canvas = GetComponentInParent<Canvas>();
        _canvasTransform = (RectTransform)canvas.transform;
        _mvpMatrix = _canvasTransform.localToWorldMatrix;
        _modules = new Module[_gridSize.x, _gridSize.y];
    }

    private void Start()
    {
        ModuleSystem.Instance.RegisterGrid(this);
    }

    private void OnValidate()
    {
        RecalculateSize();
    }

    private async void RecalculateSize()
    {
        await Task.Yield();
        float height = _tileSize.y * _gridSize.y;
        float width = _tileSize.x * _gridSize.x;
        _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    public bool ContainsPoint(Vector2 point)
    {
        Vector2 localPoint = point - (Vector2)_transform.localPosition;
        return _transform.rect.Contains(localPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = _mvpMatrix;
        Vector2 tileSize = _tileSize;

        Vector3 start = new(_xStart, _yStart, 0);
        Vector3 end = new(_xStart, _yEnd, 0);

        Gizmos.color = Color.green;
        for (int i = 0; i <= _gridSize.x; i++)
        {
            Gizmos.DrawLine(start, end);
            start.x += tileSize.x;
            end.x += _tileSize.x;
        }

        start = new(_xStart, _yStart, 0);
        end = new(_xEnd, _yStart, 0);

        for (int i = 0; i <= _gridSize.y; i++)
        {
            Gizmos.DrawLine(start, end);
            start.y += _tileSize.y;
            end.y += _tileSize.y;
        }
    }

    public bool TryPlace(Module module, Vector2 position)
    {
        if (PointToGridIndex(position, out (int, int) indexPair) && HasSlot(indexPair, module))
        {
            Vector2 index = new(indexPair.Item1, indexPair.Item2);
            Vector2 anchor = _transform.rect.center;
            Vector2 localRectMin = _transform.rect.min + index * _tileSize;
            Vector2 localRectMax = localRectMin + module.Size * _tileSize;
            Vector2 localAnchor = (localRectMin + localRectMax) / 2;
            localAnchor.y *= -1;

            Vector2 finalPosition = anchor - localAnchor + (Vector2)_transform.localPosition + (ModuleSystem.DragPosition * _tileSize);
            module.transform.localPosition = finalPosition;

            Place?.Invoke(module);
            return true;
        }

        return false;
    }

    private bool PointToGridIndex(Vector2 position, out (int, int) index)
    {
        index = (0, 0);
        Vector2 localPosition = position - (Vector2)_transform.localPosition;
        if (!_transform.rect.Contains(localPosition))
            return false;

        Vector2 relativePosition = localPosition - _transform.rect.min;
        float x = relativePosition.x / _tileSize.x;
        float y = relativePosition.y / _tileSize.y;
        index = new(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        return true;
    }

    private bool HasSlot((int, int) index, Module module)
    {
        Vector2Int startIndex = new Vector2Int(index.Item1, index.Item2) - ModuleSystem.DragPosition;
        Vector2Int endIndex = startIndex + module.Size;

        if (!HasIndex(startIndex) || !HasIndex(endIndex))
            return false;

        for (int x = startIndex[0]; x < endIndex[0]; x++)
        {
            for (int y = startIndex[1]; y < endIndex[1]; y++)
            {
                if (_modules[x, y] != null && _modules[x, y] != module)
                    return false;
            }
        }

        return true;
    }

    private bool HasIndex(Vector2Int index)
    {
        return index[0] >= 0 && index[0] <= _gridSize.x
            && index[1] >= 0 && index[1] <= _gridSize.y;
    }
}
