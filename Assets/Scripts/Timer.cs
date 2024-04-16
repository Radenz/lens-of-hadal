using System;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    private readonly List<TimerHandle> _handles = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        List<TimerHandle> disposedHandles = new();

        foreach (TimerHandle handle in _handles)
        {
            if (handle.Cancelled)
            {
                disposedHandles.Add(handle);
                continue;
            }

            handle.CurrentTime += Time.deltaTime;

            if (handle.CurrentTime >= handle.TargetTime)
            {
                handle.Callback();
                disposedHandles.Add(handle);
            }
        }

        foreach (TimerHandle handle in disposedHandles)
        {
            RemoveHandle(handle);
        }
    }

    public TimerSignal SetTimer(Action callback, float seconds)
    {
        TimerHandle handle = new()
        {
            CurrentTime = 0,
            TargetTime = seconds,
            Callback = callback,
            Cancelled = false
        };

        _handles.Add(handle);

        return new(() => CancelHandle(handle));
    }

    private void CancelHandle(TimerHandle handle)
    {
        handle.Cancelled = true;
    }

    private void RemoveHandle(TimerHandle handle)
    {
        _handles.Remove(handle);
    }
}

public class TimerHandle
{
    public float CurrentTime;
    public float TargetTime;
    public bool Cancelled;
    public Action Callback;
}

public class TimerSignal
{
    private Action _cancel;

    public TimerSignal(Action cancel)
    {
        _cancel = cancel;
    }

    public void Cancel()
    {
        if (_cancel != null)
        {
            _cancel();
            Dispose();
        }
    }

    public void Dispose()
    {
        _cancel = null;
    }
}
