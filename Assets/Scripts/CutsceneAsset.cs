using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Create Cutscene", fileName = "Cutscene", order = 0)]
public class CutsceneAsset : ScriptableObject
{
    [SerializeField] private List<Cue> _cues;
    public List<Cue> Cues => _cues;

    public EventChannelAsset onCompletedEvent;

    [NonSerialized] public bool HasSeen;
}

[System.Serializable]
public class Cue
{
    [SerializeField] private AudioClip _audioCue;
    [SerializeField, TextArea(1, 5)] private string _textCue;

    public AudioClip AudioCue => _audioCue;
    public string TextCue => _textCue;
}