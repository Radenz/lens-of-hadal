using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScanProgressBar : MonoBehaviour
{
    [SerializeField]
    private Light2D _glowLight;
    [SerializeField]
    private Transform _textTransform;

    public Sprite Sprite;
    public float YOffset;

    private void Start()
    {
        Vector3 position = _textTransform.localPosition;
        position.y = YOffset;
        _textTransform.localPosition = position;
        _glowLight.lightCookieSprite = Sprite;
    }
}
