using DG.Tweening;
using UnityEngine;

public class ExpBar : MonoBehaviour
{
    private Transform _transform;
    private readonly ManagedTween _tween = new();

    private void Start()
    {
        _transform = transform;
        EventManager.Instance.ExpChanged += OnExpChanged;
        OnExpChanged(0);
    }

    private void OnExpChanged(int _)
    {
        _tween.Kill();
        _tween.Play(_transform.DOScaleX(LevelManager.Instance.ExpPortion, 0.3f));
    }
}
