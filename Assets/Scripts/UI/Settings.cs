using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Settings : Singleton<Settings>
{
    public static float MusicVolume = 1f;
    public static float SFXVolume = 1f;
    public static float HUDOpacity = 1f;

    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private Image _scrim;

    public void Open()
    {
        RectTransform rectTransform = (RectTransform)_panel.transform;
        float height = rectTransform.rect.height;
        rectTransform.anchoredPosition = rectTransform.anchoredPosition.With(y: height);

        _container.SetActive(true);
        _scrim.raycastTarget = true;
        rectTransform.DOAnchorPosY(0, 0.2f).SetUpdate(true);
    }

    public void Close()
    {
        RectTransform rectTransform = (RectTransform)_panel.transform;
        float height = rectTransform.rect.height;

        rectTransform.DOAnchorPosY(height, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            _scrim.raycastTarget = false;
            _container.SetActive(false);
        });
    }

    // TODO: emit event
    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
    }

    public void SetHUDOpacity(float opacity)
    {
        HUDOpacity = opacity;
    }
}
