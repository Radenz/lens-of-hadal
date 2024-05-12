using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

// TODO: immediate remove when module is dragged
[RequireComponent(typeof(RectTransform))]
public class ModuleGrid : MonoBehaviour
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    private RectTransform _transform;
    private RectTransform _canvasTransform;
    private Matrix4x4 _mvpMatrix;

    [SerializeField]
    private Vector2Int _gridSize;

    private Module[,] _modules;

    [SerializeField]
    private Vector2 _tileSize = new(160, 160);

    public event Action<ModuleGrid, Module> Place;

    private static event Action<Module> PlaceInventory;

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

        if (Name == "Inventory")
            PlaceInventory += Add;
    }

    private void Start()
    {
        ModuleSystem.Instance.RegisterGrid(this);
    }

    private void OnValidate()
    {
        RecalculateSize();
    }

    [Button]
    private void LogModules()
    {
        string repr = "";
        for (int y = _gridSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                repr += _modules[x, y] != null ? "1" : "0";
            }
            repr += "\n";
        }

        Debug.Log(repr);
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

    public void Add(Module module)
    {
        // ? Iterate from top left
        for (int y = _gridSize.y - 1; y >= 0; y--)
            for (int x = 0; x < _gridSize.x; x++)
            {
                if (HasSlot((x, y), module))
                {
                    PlaceModule(module, new(x, y));
                    return;
                }
            }
    }

    public void Expand((int, int) size)
    {
        Vector2Int initialSize = _gridSize;
        _gridSize = new(size.Item1, size.Item2);
        Module[,] modules = _modules;
        _modules = new Module[_gridSize.x, _gridSize.y];

        Dictionary<Module, Vector2Int> modulePositions = new();

        for (int y = initialSize.y - 1; y >= 0; y--)
            for (int x = 0; x < initialSize.x; x++)
            {
                if (modules[x, y] != null)
                {
                    modulePositions[modules[x, y]] = new(x, _gridSize.y - 1 - y);
                }
            }
        ReadjustSizeAndModules(modulePositions);
    }

    private async void ReadjustSizeAndModules(Dictionary<Module, Vector2Int> modulePositions)
    {
        await Task.Yield();
        float height = _tileSize.y * _gridSize.y;
        float width = _tileSize.x * _gridSize.x;
        _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        await Awaitable.NextFrameAsync();
        foreach (KeyValuePair<Module, Vector2Int> entry in modulePositions)
        {
            PlaceModule(entry.Key, entry.Value);
        }
    }

    public void Remove(Module module)
    {
        for (int y = _gridSize.y - 1; y >= 0; y--)
            for (int x = 0; x < _gridSize.x; x++)
            {
                if (_modules[x, y] == module)
                    _modules[x, y] = null;
            }
    }

    public bool TryPlace(Module module, Vector2 position)
    {
        if (PointToGridIndex(position, out (int, int) indexPair) && HasSlot(indexPair, module))
        {
            Vector2Int placementIndex = new(indexPair.Item1, indexPair.Item2);
            PlaceModule(module, placementIndex - ModuleSystem.DragPosition);
            return true;
        }

        return false;
    }

    private void PlaceModule(Module module, Vector2Int index)
    {
        Vector2 rectBottomLeft = _transform.rect.center - _transform.rect.size / 2;
        Vector2 bottomLeft = rectBottomLeft + index * _tileSize;
        Vector2 topRight = bottomLeft + module.Size * _tileSize;
        Vector2 localPosition = (bottomLeft + topRight) / 2;
        Vector2 finalPosition = localPosition + (Vector2)_transform.localPosition;
        module.transform.localPosition = finalPosition;

        if (module.Grid != null)
            module.Grid.Remove(module);

        if (Name == "Upgrade")
            EnsureNoConflictingModules(module);

        for (int i = index[0]; i < index[0] + module.Size.x; i++)
        {
            for (int j = index[1]; j < index[1] + module.Size.y; j++)
            {
                _modules[i, j] = module;
            }
        }

        module.Grid = this;
        Place?.Invoke(this, module);
    }

    private void EnsureNoConflictingModules(Module module)
    {
        for (int y = _gridSize.y - 1; y >= 0; y--)
            for (int x = 0; x < _gridSize.x; x++)
            {
                if (_modules[x, y] != null && _modules[x, y].GroupTag == module.GroupTag)
                    PlaceToInventory(_modules[x, y]);
            }
    }

    private void PlaceToInventory(Module module)
    {
        PlaceInventory?.Invoke(module);
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
