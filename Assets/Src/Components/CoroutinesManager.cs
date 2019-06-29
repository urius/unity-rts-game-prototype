
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

internal interface ICoroutinesManager
{
    void StartCoroutine(IEnumerator routine);
    void StopAllComponentCoroutines();
}

public class CoroutinesManager : IDisposable, ICoroutinesManager
{
    [Inject]
    private CoroutinesHolder _coroutinesHolder;

    private List<Coroutine> _coroutinesList;
    public CoroutinesManager()
    {
        _coroutinesList = new List<Coroutine>();
    }

    public void StartCoroutine(IEnumerator routine)
    {
        _coroutinesList.Add(_coroutinesHolder.StartCoroutine(routine));
    }

    public void StopAllComponentCoroutines()
    {
        if (_coroutinesHolder != null)
        {
            _coroutinesList.ForEach(_coroutinesHolder.StopCoroutine);
        }
    }

    public void Dispose()
    {
        StopAllComponentCoroutines();
    }

}