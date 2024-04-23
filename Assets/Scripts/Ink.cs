using DG.Tweening;
using UnityEngine;

public class Ink : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer
            .DOFade(0, _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Splat.Instance.Activate();
        }
    }
}
