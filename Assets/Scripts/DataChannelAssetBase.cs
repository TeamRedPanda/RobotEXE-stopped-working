using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DataChannelAssetBase<TData> : ScriptableObject
{
    public event Action<TData> OnData;

    public void Emit(TData data)
    {
        OnData?.Invoke(data);
    }
    
    public Task<TData> NotifyOnceAsync(CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<TData>();

        Action<TData> OnEventCallback = null;
        OnEventCallback = (data) =>
        {
            tcs.TrySetResult(data);
            OnData -= OnEventCallback;
        };
        OnData += OnEventCallback;

        if (cancellationToken.CanBeCanceled)
            cancellationToken.Register(() =>
            {
                tcs.TrySetCanceled();
                OnData -= OnEventCallback;
            });

        return tcs.Task;
    }
}
