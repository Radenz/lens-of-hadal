using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class Module : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Vector2Int _size;
    public Vector2Int Size => _size;

    [SerializeField]
    private Vector2Int _tileSize = new(120, 120);

    private RectTransform _canvasTransform;
    private RectTransform _transform;
    private bool _isDragged = false;
    private Vector2 _initialPosition;
    private Vector2 _initialPointerPosition;

    public ModuleGrid Grid;

    private void Awake()
    {
        _transform = (RectTransform)transform;
        _canvasTransform = (RectTransform)GetComponentInParent<Canvas>().transform;
    }

    private void Update()
    {
        HandleDrag();
    }

    private void OnValidate()
    {
        RecalculateSize();
    }

    private async void RecalculateSize()
    {
        await Task.Yield();

        if (_transform == null) return;

        float height = _tileSize.y * _size.y;
        float width = _tileSize.x * _size.x;
        _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ModuleSystem.DraggedModule = this;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            Input.mousePosition,
            null,
            out _initialPointerPosition
        );

        _initialPosition = _transform.localPosition;
        _isDragged = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _transform,
            Input.mousePosition,
            null,
            out Vector2 rectPosition
        );

        Vector2 relativePosition = rectPosition - _transform.rect.min;
        float x = relativePosition.x / _tileSize.x;
        float y = relativePosition.y / _tileSize.y;

        Vector2Int index = new(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        ModuleSystem.DragPosition = index;
    }

    private void HandleDrag()
    {
        if (!_isDragged) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            Input.mousePosition,
            null,
            out Vector2 pointerPosition
        );
        Vector2 delta = pointerPosition - _initialPointerPosition;
        _transform.localPosition = _initialPosition + delta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isDragged) return;
        _isDragged = false;
        ModuleSystem.DraggedModule = null;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            Input.mousePosition,
            null,
            out Vector2 pointerPosition
        );

        if (ModuleSystem.Instance.TryPlaceModule(this, pointerPosition))
            return;

        _transform.localPosition = _initialPosition;

    }
}
