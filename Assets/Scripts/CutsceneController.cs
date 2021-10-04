using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private TextCueChannelAsset _textCueChannel;

    private CutsceneAsset _currentCutscene;
    private int currentCue = 0;

    public void StartCutscene(CutsceneAsset cutscene)
    {
        _currentCutscene = cutscene;
        currentCue = 0;
        
        _textCueChannel.EmitNextCue(_currentCutscene.Cues[currentCue++].TextCue);

        _textCueChannel.OnCueCompleted += () =>
        {
            if (currentCue >= _currentCutscene.Cues.Count)
                return;
            
            _textCueChannel.EmitNextCue(_currentCutscene.Cues[currentCue++].TextCue);
        };
    }
}
