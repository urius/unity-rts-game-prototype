using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class UnitMainController : IInitializable, IDisposable
{
    [Inject]
    private SignalBus _signalBus;
    [Inject]
    private CoroutinesManager _coroutinesManager;
    [Inject]
    private GameObject _gameObject;
    [Inject]
    private UnitModel _model;

    public void Initialize()
    {
        _model.UnitDestroyed += OnUnitDestroyed;

        _signalBus.Fire(new UnitAddedSignal { unit = _model });
    }

    private void OnUnitDestroyed()
    {
        _coroutinesManager.StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(10);
        GameObject.Destroy(_gameObject);
    }

    public void Dispose()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;
    }
}
