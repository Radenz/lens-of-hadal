using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action Execute;

    private bool _disabled = false;

    [Header("Look")]
    [SerializeField]
    private string _text;
    [SerializeField]
    private Color _tint;
    [SerializeField]
    private Color _backgroundTint = new(68f / 255f, 68f / 255f, 68f / 255f);

    [Header("Interaction")]
    [SerializeField]
    private float _holdTime;

    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _frame;
    [SerializeField]
    private Image _progressBar;
    [SerializeField]
    private Transform _progressBarTransform;

    [Header("State")]
    [SerializeField, ReadOnly]
    private float _progressTime;
    [SerializeField, ReadOnly]
    private bool _isHolding;
    [SerializeField, ReadOnly]
    private bool _active = true;

    private Color _frameBaseTint = new(89f / 255f, 86f / 255f, 82f / 255f);

    private void Update()
    {
        if (!_active) return;

        if (_isHolding)
        {
            _progressTime += Time.deltaTime;
            Vector3 scale = _progressBarTransform.localScale;
            scale.x = _progressTime / _holdTime;
            _progressBarTransform.localScale = scale;
        }
        else
        {
            _progressTime -= Time.deltaTime;
            ClampProgressTime();
            Vector3 scale = _progressBarTransform.localScale;
            scale.x = _progressTime / _holdTime;
            _progressBarTransform.localScale = scale;
        }

        if (_progressTime >= _holdTime)
        {
            ClampProgressTime();
            _active = false;
            _isHolding = false;
            Execute?.Invoke();
        }
    }

    private void OnValidate()
    {
        _frame.color = _tint;
        _progressBar.color = _tint * _frameBaseTint;
        _background.color = _progressBar.color * _backgroundTint;
        _label.text = _text;
    }

    private void ClampProgressTime()
    {
        if (_progressTime < 0)
            _progressTime = 0;
        if (_progressTime > _holdTime)
            _progressTime = _holdTime;
    }

    public void OnPointerDown(PointerEventData _)
    {
        if (_disabled) return;
        _isHolding = true;
    }

    public void OnPointerUp(PointerEventData _)
    {
        if (_disabled) return;
        _isHolding = false;
    }

    public void Disable()
    {
        _frame.color = _frame.color.With(a: 0.5f);
        _progressBar.color = _progressBar.color.With(a: 0.5f);
        _background.color = _background.color.With(a: 0.5f);
        _label.color = _label.color.With(a: 0.5f);

        _disabled = true;
    }
}
