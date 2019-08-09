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
    private UnitFacade _facade;

    public void Initialize()
    {
        _facade.UnitModel.UnitDestroyed += OnUnitDestroyed;

        _signalBus.Fire(new UnitAddedSignal { unit = _facade });
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
        _facade.UnitModel.UnitDestroyed -= OnUnitDestroyed;
    }
}
