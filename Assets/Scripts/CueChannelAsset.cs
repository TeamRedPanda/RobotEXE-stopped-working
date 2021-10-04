using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CueChannelAssetBase<TData> : ScriptableObject
{
    public event Action<TData> NextCue;
    public event Action OnCueCompleted;

    public void EmitNextCue(TData data)
    {
        NextCue?.Invoke(data);
    }

    public void EmitCueCompleted()
    {
        OnCueCompleted?.Invoke();
    }
}

public static class ActionExtensions
{
    public static Task NotifyOnceAsync(this Action action, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<bool>();

        Action OnEventCallback = null;
        OnEventCallback = () =>
        {
            tcs.TrySetResult(true);
            action -= OnEventCallback;
        };
        action += OnEventCallback;

        if (cancellationToken.CanBeCanceled)
            cancellationToken.Register(() =>
            {
                tcs.TrySetCanceled();
                action -= OnEventCallback;
            });

        return tcs.Task;
    }

    public static Task<TData> NotifyOnceAsync<TData>(this Action<TData> action,
        CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<TData>();

        Action<TData> OnEventCallback = null;
        OnEventCallback = (data) =>
        {
            tcs.TrySetResult(data);
            action -= OnEventCallback;
        };
        action += OnEventCallback;

        if (cancellationToken.CanBeCanceled)
            cancellationToken.Register(() =>
            {
                tcs.TrySetCanceled();
                action -= OnEventCallback;
            });

        return tcs.Task;
    }
}
