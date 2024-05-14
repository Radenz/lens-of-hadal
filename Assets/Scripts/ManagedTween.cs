using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ManagedTween
{
    private Tween tween;
    public bool IsPlaying = false;
    private List<Tween> queue = new();

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
            Dequeue();
        });

        tween.Play();
    }

    public void PlayNext(Tween animation)
    {
        if (IsPlaying)
        {
            animation.Pause();
            queue.Add(animation);
        }
        else
            Play(animation);
    }

    private void Dequeue()
    {
        if (queue.Count == 0) return;
        Tween animation = queue[0];
        queue.RemoveAt(0);
        Play(animation);
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
