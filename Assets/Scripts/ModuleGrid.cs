using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class ModuleGrid : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private RectTransform _transform;
    private RectTransform _canvasTransform;
    private Matrix4x4 _mvpMatrix;

    [SerializeField]
    private Vector2Int _gridSize;

    private Module[,] _modules;

    [SerializeField]
    private Vector2 _tileSize
        => new(_transform.rect.width / _gridSize.x, _transform.rect.height / _gridSize.y);

    [SerializeField]
    bool _logOnce = true;

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

    private void Update()
    {
        if (ModuleSystem.DraggedModule != null)
            UpdateSlotEligiblity();
    }

    private void UpdateSlotEligiblity()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            Input.mousePosition,
            null,
            out Vector2 position
        );

        if (_transform.rect.Contains(position))
            ModuleSystem.HoveredModuleGrid = this;
        else if (ModuleSystem.HoveredModuleGrid == this)
            ModuleSystem.HoveredModuleGrid = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ModuleSystem.HoveredModuleGrid = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ModuleSystem.HoveredModuleGrid = null;
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

        _logOnce = false;
    }

    public bool TryPlace(Module module, Vector2 position)
    {
        if (PointToGridIndex(position, out (int, int) indexPair))
        {
            Vector2 tileSize = _tileSize;
            Vector2 index = new(indexPair.Item1, indexPair.Item2);
            // TODO: adjust anchor to center and based on module size
            Vector2 anchor = _transform.rect.min + tileSize * index;

            RectTransform moduleTransform = (RectTransform)module.transform;
            Vector2 localAnchor = moduleTransform.rect.min;
            Vector2 finalPosition = anchor - localAnchor + (Vector2)_transform.localPosition;
            module.transform.localPosition = finalPosition;

            // TODO: notify module has been placed

            return true;
        }

        return false;
    }

    private bool PointToGridIndex(Vector2 position, out (int, int) index)
    {
        index = (0, 0);
        if (!_transform.rect.Contains(position))
            return false;

        Vector2 normalizedPoint = Rect.PointToNormalized(_transform.rect, position);
        normalizedPoint *= _gridSize;
        index = (Mathf.FloorToInt(normalizedPoint.x), Mathf.FloorToInt(normalizedPoint.y));
        return true;
    }


}
