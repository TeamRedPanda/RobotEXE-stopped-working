using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Typewriter : MonoBehaviour, PlayerControls.IUIActions
{
    private PlayerControls _playerControls;
    
    [SerializeField, Range(0, 1)] private float textWaitTime = 0.03f;
    private TextMeshProUGUI _textBox;

    [SerializeField] private TextCueChannelAsset textCueChannel;

    private bool _isSentenceComplete;
    private string _currentText = String.Empty;

    void Awake()
    {
        _textBox = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _playerControls = new PlayerControls();
        _playerControls.UI.SetCallbacks(this);
        _playerControls.UI.Enable();

        textCueChannel.NextCue += ShowText;
    }

    private void OnDisable()
    {
        _playerControls?.Dispose();
        _playerControls = null;

        textCueChannel.NextCue -= ShowText;
    }

    private void ShowText(string text)
    {
        _currentText = text;
        StartCoroutine(AnimateText(text));
    }

    public void SkipToNextText()
    {
        StopAllCoroutines();

        if (!_isSentenceComplete)
        {
            _textBox.maxVisibleCharacters = _currentText.Length;
            _isSentenceComplete = true;
            return;
        }
    
        _textBox.SetText(String.Empty);
        textCueChannel.EmitCueCompleted();
    }

    IEnumerator AnimateText(string displayText)
    {
        _isSentenceComplete = false;
        _textBox.SetText(displayText);

        for (int i = 0; i <= (displayText.Length); i++)
        {
            _textBox.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textWaitTime);
        }

        _isSentenceComplete = true;
    }
    
    public void OnAdvance(InputAction.CallbackContext context)
    {
        if (context.performed)
            SkipToNextText();
    }
}
