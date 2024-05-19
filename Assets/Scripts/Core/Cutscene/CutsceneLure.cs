using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CutsceneLure : MonoBehaviour
{
    [SerializeField]
    private Light2D _light;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        PlayCutscene();
    }

    private async void PlayCutscene()
    {
        DOTween.To(
            () => _light.intensity,
            intensity => _light.intensity = intensity,
            2f,
            1f
        ).SetEase(Ease.Linear);

        _transform.DOMoveY(_transform.position.y + 0.5f, 0.5f)
            .SetEase(Ease.InOutQuad)
            .SetLoops(2, LoopType.Yoyo);
        _transform.DOMoveX(_transform.position.x + 2, 2).SetEase(Ease.InQuad);

        await Awaitable.WaitForSecondsAsync(1);

        DOTween.To(
            () => _light.intensity,
            intensity => _light.intensity = intensity,
            0f,
            1f
        ).SetEase(Ease.Linear);

        await Awaitable.WaitForSecondsAsync(1);

        Destroy(gameObject);
    }
}
