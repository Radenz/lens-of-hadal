using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource _genericPlayer;

    [SerializeField]
    private AudioSource _bgmBackgroundLayer;

    [SerializeField]
    private AudioSource _bgmBossfightLayer;

    [SerializeField]
    private float _crossfadeDuration = 2f;

    private void Start()
    {
        PlayBgm();
    }

    public void PlayBgm()
    {
        _bgmBackgroundLayer.Play();
        _bgmBossfightLayer.Play();
        _bgmBossfightLayer.volume = 0;
        _bgmBackgroundLayer.loop = true;
    }

    // TODO: layering
    public void PlaySFX(AudioClip audioClip)
    {
        _genericPlayer.PlayOneShot(audioClip, Settings.SFXVolume);
    }

    [Button]
    public void PlayBossfightLayer()
    {
        DOTween.To(
            () => _bgmBossfightLayer.volume,
            volume => _bgmBossfightLayer.volume = volume,
            1f,
            _crossfadeDuration
        )
            .OnUpdate(() =>
            {
                _bgmBackgroundLayer.volume = 1 - _bgmBossfightLayer.volume;
            })
            .SetEase(Ease.Linear);
    }

    [Button]
    public void StopBossfightLayer()
    {
        DOTween.To(
            () => _bgmBackgroundLayer.volume,
            volume => _bgmBackgroundLayer.volume = volume,
            1f,
            _crossfadeDuration
        )
            .OnUpdate(() =>
            {
                _bgmBossfightLayer.volume = 1 - _bgmBackgroundLayer.volume;
            })
            .SetEase(Ease.Linear);
    }
}
