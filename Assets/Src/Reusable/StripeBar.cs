using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripeBar : MonoBehaviour
{
    [SerializeField]
    private bool _destroyIfLessThanZero = false;
    [SerializeField]
    private float _destroyDelaySeconds = 0f;
    private SpriteRenderer _spriteRenderer;
    private Transform _dynamicPart;

    private bool _started = false;
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _dynamicPart = _spriteRenderer.transform.parent.transform;
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetPercent(float value, bool affectColor = true)
    {

        var relativeValue = Mathf.Clamp(value / 100, 0, 1);
        _dynamicPart.transform.localScale = new Vector3(relativeValue, 1, 1);
        if (affectColor)
        {
            SetColor(Color.Lerp(Color.green, Color.red, 1 - relativeValue));
        }

        
        if (_destroyIfLessThanZero && value <= 0)
        {
            StartCoroutine(WaitAndDestroyCoroutine());
        }
    }

    private IEnumerator WaitAndDestroyCoroutine()
    {
        yield return new WaitForSeconds(_destroyDelaySeconds);
        Destroy(this.gameObject);
    }

    public void SetColor(Color value)
    {
        _spriteRenderer.color = value;
    }
}
