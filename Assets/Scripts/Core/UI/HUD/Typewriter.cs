using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Typewriter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private TextMeshProUGUI _skipNextButton;

    [SerializeField]
    [ResizableTextArea]
    private List<string> _texts;
    [SerializeField]
    private float _speed;

    [SerializeField]
    private bool _pauseTime = false;

    private float Delay => 1f / _speed;

    private float _timeSinceLastChar = 0f;
    private int _index = -1;
    private bool _isFinished = false;
    private bool _isTyping = false;
    private int _charIndex = 0;

    public event Action Completed;

    private void Start()
    {
        if (_pauseTime)
            Time.timeScale = 0f;
        Next();
    }

    private void Update()
    {
        if (!_isTyping) return;
        _timeSinceLastChar += Time.deltaTime;

        if (_timeSinceLastChar >= Delay)
        {
            while (_timeSinceLastChar >= Delay)
            {
                _timeSinceLastChar -= Delay;
                TypeNextChar();
            }
        }
    }

    public void SkipOrNext()
    {
        if (_isFinished)
        {
            Next();
        }
        else
        {
            Skip();
        }
    }

    public void Skip()
    {
        _isTyping = false;
        _isFinished = true;
        _label.text = _texts[_index];
        _skipNextButton.text = "NEXT";
    }

    public void Next()
    {
        _index++;
        if (_texts.Count <= _index)
        {
            Completed?.Invoke();
            if (_pauseTime)
                Time.timeScale = 1f;
            return;
        }
        Type();
    }

    private void Type()
    {
        _isFinished = false;
        _isTyping = true;
        _label.text = "";
        _skipNextButton.text = "SKIP";
        _charIndex = 0;
        _timeSinceLastChar = 0f;
    }

    private void TypeNextChar()
    {
        _charIndex++;

        if (_charIndex > _texts[_index].Length)
        {
            _isFinished = true;
            _isTyping = false;
            _skipNextButton.text = "NEXT";
            return;
        }


        string text = _texts[_index];
        string substring = text[.._charIndex];
        _label.text = substring;
    }
}
