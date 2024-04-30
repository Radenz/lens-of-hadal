using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Vector2Int _size;

    private RectTransform _canvasTransform;
    private RectTransform _transform;
    private bool _isDragged = false;
    private Vector2 _initialPosition;
    private Vector2 _initialPointerPosition;

    private void Awake()
    {
        _canvasTransform = (RectTransform)GetComponentInParent<Canvas>().transform;
        _transform = (RectTransform)transform;
    }

    private void Update()
    {
        HandleDrag();
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

        if (ModuleSystem.HoveredModuleGrid
            ?.TryPlace(this, (Vector2)_transform.localPosition)
            ?? false)
        {
            return;
        }

        _transform.localPosition = _initialPosition;

    }
}
