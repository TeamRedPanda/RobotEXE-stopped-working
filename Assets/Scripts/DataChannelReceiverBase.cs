using System;
using UnityEngine;
using UnityEngine.Events;

public class DataChannelReceiverBase<TData> : MonoBehaviour
{
    [SerializeField, SerializeReference] private DataChannelAssetBase<TData> Channel;
    [SerializeField] private UnityEvent<TData> OnData;

    private void OnEnable()
    {
        Channel.OnData += OnDataCallback;
    }

    private void OnDisable()
    {
        Channel.OnData -= OnDataCallback;
    }

    private void OnDataCallback(TData data)
    {
        OnData?.Invoke(data);
    }
}
