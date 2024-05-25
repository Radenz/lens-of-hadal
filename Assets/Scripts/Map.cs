using UnityEngine;

public class Map : Singleton<Map>
{
    [SerializeField]
    private AudioClip _pageFlipAudio;
    [SerializeField]
    private Canvas _canvas;

    public void Show()
    {
        // _canvas.enabled = true;
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(_pageFlipAudio);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySFX(_pageFlipAudio);
        // _canvas.enabled = false;
    }
}
