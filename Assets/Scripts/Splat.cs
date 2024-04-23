using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Splat : MonoBehaviour
{
    public static Splat Instance;

    [SerializeField]
    private Image _image;
    [SerializeField]
    private float _duration;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetAlpha(0);
    }

    public void Activate()
    {
        DOTween.Kill(_image);
        SetAlpha(1);
        _image.DOFade(0, _duration).SetEase(Ease.InQuad);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _image.color;
        color.a = alpha;
        _image.color = color;
    }
}
