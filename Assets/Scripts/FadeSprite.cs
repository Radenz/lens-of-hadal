using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeSprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sprite;
    [SerializeField]
    private float _alpha;
    [SerializeField]
    private float _duration;

    private void Start()
    {
        _sprite.DOFade(_alpha, _duration).SetEase(Ease.InQuad);
    }
}
