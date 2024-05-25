using DG.Tweening;
using UnityEngine;

public class SonarOutline : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _outlineRenderer;
    [SerializeField]
    private AudioSource _pingAudio;
    private void Start()
    {
        _pingAudio.volume = Settings.SFXVolume * 0.2f;
        _outlineRenderer.DOFade(0, 0.8f);
    }
}
