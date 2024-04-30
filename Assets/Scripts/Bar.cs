using DG.Tweening;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private float _maxValue = 1f;
    private float _value = 1f;

    [SerializeField]
    private GameObject _displayBar;

    private ManagedTween _tween = new();

    public float MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;
            RecomputeDisplay();
        }
    }

    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            RecomputeDisplay();
        }
    }

    private void RecomputeDisplay()
    {
        Vector3 scale = _displayBar.transform.localScale;
        scale.x = _value / _maxValue;
        _tween.Kill();
        _tween.Play(_displayBar.transform.DOScaleX(scale.x, 0.3f));
    }
}
