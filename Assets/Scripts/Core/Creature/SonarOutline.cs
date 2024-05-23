using DG.Tweening;
using UnityEngine;

public class SonarOutline : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _outlineRenderer;
    private void Start()
    {
        _outlineRenderer.DOFade(0, 0.8f);
    }
}
