using System;
using DG.Tweening;
using UnityEngine;

public class ManagedTween
{
    private Tween tween;
    public bool IsPlaying = false;

    public void Play(Tween animation)
    {
        if (IsPlaying)
            Debug.LogWarning("Trying to play managed tween before previous tween is killed.");

        IsPlaying = true;
        tween = animation;

        tween.OnKill(() =>
        {
            IsPlaying = false;
            tween = null;
        });

        tween.Play();
    }

    public void Kill()
    {
        if (IsPlaying)
        {
            IsPlaying = false;
            tween?.Kill();
        }
    }
}
