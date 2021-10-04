using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneStartTrigger : MonoBehaviour
{
    [SerializeField] private CutsceneAsset _cutscene;
    [SerializeField] private CutsceneChannelAsset _cutsceneChannel;
    [SerializeField] private bool _fireOnce;

    private void Awake()
    {
        if (_fireOnce && _cutscene.HasSeen)
            Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _cutsceneChannel.Emit(_cutscene);
        if (_fireOnce)
            Destroy(gameObject);
    }
}
