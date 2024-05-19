using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private float _maxValue = 1f;
    private float _value = 1f;

    [SerializeField]
    private GameObject _displayBar;

    [SerializeField]
    private Image _frame;
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _fill;

    private ManagedTween _tween = new();
    private ManagedTween _fadeTween = new();

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

    private void Start()
    {
        _frame.color = _frame.color.With(a: 0);
        _background.color = _background.color.With(a: 0);
        _fill.color = _fill.color.With(a: 0);
        SetAlpha(0f);
    }

    private void RecomputeDisplay()
    {
        Vector3 scale = _displayBar.transform.localScale;
        scale.x = _value / _maxValue;
        _tween.Kill();
        _tween.Play(_displayBar.transform.DOScaleX(scale.x, 0.3f));

        _fadeTween.Kill();
        _fadeTween.Play(SetAlpha(_value == _maxValue ? 0f : 1f));
    }

    private Tween SetAlpha(float alpha)
    {
        return DOTween.Sequence()
            .Join(_frame.DOFade(alpha, 0.3f))
            .Join(_background.DOFade(alpha, 0.3f))
            .Join(_fill.DOFade(alpha, 0.3f));
    }

}
