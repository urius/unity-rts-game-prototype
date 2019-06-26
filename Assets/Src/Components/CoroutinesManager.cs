
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

public class CoroutinesManager : IInitializable, IDisposable, ICoroutinesManager
{
    [Inject]
    private CoroutinesHolder _coroutinesHolder;

    private List<Coroutine> _coroutinesList;
    public void Initialize()
    {
        _coroutinesList = new List<Coroutine>();
    }

    public void StartCoroutine(IEnumerator routine) {
        _coroutinesList.Add(_coroutinesHolder.StartCoroutine(routine));
    }

    public void StopAllComponentCoroutines() {
        _coroutinesList.ForEach(_coroutinesHolder.StopCoroutine);
    }

    public void Dispose()
    {
        _coroutinesList.ForEach(_coroutinesHolder.StopCoroutine);
    }

}