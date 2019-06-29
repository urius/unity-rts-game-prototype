using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitAIAttackController : IInitializable, IDisposable
{
    [Inject]
    private GameData _gameData;
    [Inject]
    private CoroutinesManager _coroutinesManager;
    [Inject]
    private UnitsCollectionProvider _unitsCollectionProvider;
    [Inject]
    private UnitModel _model;

    public void Initialize()
    {
        _coroutinesManager.StartCoroutine(ChooseAttackTargetCoroutine());
    }
    public void Dispose()
    {
        _coroutinesManager?.StopAllComponentCoroutines();
    }

    private IEnumerator ChooseAttackTargetCoroutine()
    {
        while (true)
        {
            var attackTarget = GetClosestAttackableEnemy(UnitsCollectionProvider._units);
            _model.attackTarget = attackTarget;

            yield return new WaitForSeconds(1f);
        }
    }

    private UnitModel GetClosestAttackableEnemy(IEnumerable<UnitModel> unitsToIterate)
    {
        var transform = _model.transform;
        UnitModel closest = null;
        foreach (var unit in unitsToIterate)
        {
            if (_model.teamId != unit.teamId)
            {
                var distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance <= _model.detectRadius)
                {
                    if (closest == null || distance < Vector3.Distance(transform.position, closest.transform.position))
                    {
                        closest = unit;
                    }
                }
            }
        }
        return closest;
    }
}
