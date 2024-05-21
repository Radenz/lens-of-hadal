using UnityEngine;

public class DepthIndicator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _maxWorldY;
    [SerializeField]
    private float _minWorldY;
    [SerializeField]
    private float _maxIndicatorY;
    [SerializeField]
    private float _minIndicatorY;

    [Header("References")]
    [SerializeField]
    private Transform _arrowTransform;


    private float _worldRange;
    private float _indicatorRange;

    private void Start()
    {
        _worldRange = _maxWorldY - _minWorldY;
        _indicatorRange = _maxIndicatorY - _minIndicatorY;
    }

    private void Update()
    {
        float y = PlayerController.Instance.Position.y;
        float offset = y - _minWorldY;

        float indicatorY = _indicatorRange / _worldRange * offset + _minIndicatorY;
        _arrowTransform.localPosition = _arrowTransform.localPosition.With(y: indicatorY);
    }
}
