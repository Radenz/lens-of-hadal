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

    public void StartBobbing()
    {
        Transform transform_ = transform;
        float peak = transform_.position.y;
        peak += _height;
        transform_.DOMoveY(peak, _bobCount).SetLoops(_bobCount, LoopType.Yoyo);
    }
}
