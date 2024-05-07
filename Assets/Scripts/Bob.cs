using DG.Tweening;
using UnityEngine;

public class Bob : MonoBehaviour
{
    [SerializeField]
    private float _height;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private int _bobCount;

    private Tween _tween;

    public void StartBobbing()
    {
        Transform transform_ = transform;
        float peak = transform_.position.y;
        peak += _height;
        _tween = transform_
            .DOMoveY(peak, _duration)
            .SetLoops(_bobCount, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        if (!_tween.IsComplete()) _tween.Kill();
    }
}
