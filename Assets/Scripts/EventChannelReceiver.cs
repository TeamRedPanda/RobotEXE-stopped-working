using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EventChannelReceiver : MonoBehaviour
{
    [SerializeField, SerializeReference] private EventChannelAsset Channel;
    [SerializeField] private UnityEvent OnEvent;

    private void OnEnable()
    {
        Channel.OnEvent += OnDataCallback;
    }

    private void OnDisable()
    {
        Channel.OnEvent -= OnDataCallback;
    }

    private void OnDataCallback()
    {
        OnEvent?.Invoke();
    }
}
