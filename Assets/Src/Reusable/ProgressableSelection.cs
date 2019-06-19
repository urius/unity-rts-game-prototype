using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressableSelection : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float _progress = 1f;
    [SerializeField]
    private Color _progressColor;
    [SerializeField]
    private Color _bgColor;

    [SerializeField]
    private Image _progressImage;
    [SerializeField]
    private Image _bgImage;

    
    public bool isSelected
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }

    private Slider _slider;

    public float progress
    {
        get { return _progress; }
        set
        {
            _progress = Mathf.Clamp(progress, 0, 1);
            _slider.value = _progress;
        }
    }

    void Awake()
    {
        _slider = GetComponentInChildren<Slider>();

        SetColors();
        progress = _progress;
    }

    private void SetColors()
    {
        _progressImage.color = _progressColor;
        _bgImage.color = _bgColor;
    }

    // Called when some editor variable changed
    void OnValidate()
    {
        Awake();
    }
}
