using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Event channel/Void", fileName = "EventChannelAsset", order = -1)]
public class EventChannelAsset : ScriptableObject
{
    public event Action OnEvent;

    public void Emit()
    {
        OnEvent?.Invoke();
    }

    public Task NotifyOnceAsync(CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<bool>();

        Action OnEventCallback = null;
        OnEventCallback = () =>
        {
            tcs.TrySetResult(true);
            OnEvent -= OnEventCallback;
        };
        OnEvent += OnEventCallback;

        if (cancellationToken.CanBeCanceled)
            cancellationToken.Register(() =>
            {
                tcs.TrySetCanceled();
                OnEvent -= OnEventCallback;
            });
        
        return tcs.Task;
    }
}
