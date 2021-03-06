﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitAIMoveController : IInitializable, IDisposable
{
    [Inject]
    private CoroutinesManager _coroutinesManager;
    [Inject]
    private UnitsCollectionProvider _unitsCollectionProvider;
    [Inject]
    private UnitModel _model;
    [Inject]
    private AwakableView _view;

    public void Initialize()
    {
        _view.OnStartPromise.Then(OnViewStarted);
    }

    private void OnViewStarted()
    {
        _model.UnitDestroyed += OnUnitDestroyed;

        _coroutinesManager.StartCoroutine(ChooseTargetToMoveCoroutine());
    }

    private void OnUnitDestroyed()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;

        _coroutinesManager.StopAllComponentCoroutines();
    }

    private IEnumerator ChooseTargetToMoveCoroutine()
    {
        while (true)
        {
            var target = GetClosestEnemy(UnitsCollectionProvider._units);
            _model.destinationPoint = GetDestinationPoint(target);

            yield return new WaitForSeconds(3f);
        }
    }

    private Vector2 GetDestinationPoint(UnitModel target)
    {
        var moveToPosition = _model.position;
        if (target != null && target.hp > 0)
        {
            var distanceToClosest = Vector3.Distance(target.position, _model.position);

            if (target.detectRadius < _model.detectRadius)
            {
                //go to the middle between detect radiuses
                var vectorFromEnemyToMe = _model.position - target.position;
                var distanceToCome = target.detectRadius + (_model.detectRadius - target.detectRadius) / 2;

                moveToPosition = target.position + Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToCome);

                //Debug.DrawLine(transform.position + transform.up, moveToPosition, Color.green, 1f);
            }
            else //if (distanceToClosest > _unitAvatar.detectRadius)
            {
                //need to come closer
                var vectorFromEnemyToMe = _model.position - target.position;
                var distanceToCome = _model.detectRadius / 2;
                moveToPosition = target.position + Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToCome);

                //Debug.DrawLine(transform.position, moveToPosition, Color.red, 1f);
            }

            var team = _model.teamId;

            //var enemiesExceptTarget = _unitsCollectionProvider.units.FindAll(u => (u.teamId != team) && (u != target));
            var enemiesExceptTarget = UnitsCollectionProvider._units.FindAll(u => (u.UnitModel.teamId != team) && (u.UnitModel != target));
            var extraEnemiesTargetedToMe = GetUnitsTargetedMe(enemiesExceptTarget);
            if (extraEnemiesTargetedToMe.Count > 0)
            {
                //move out from danger zone
                foreach (var targettingEnemy in extraEnemiesTargetedToMe)
                {
                    var vectorFromEnemyToMe = targettingEnemy.position - _model.position;
                    var distanceToComeOut = targettingEnemy.detectRadius - vectorFromEnemyToMe.magnitude;
                    if (distanceToComeOut > 0)
                    {
                        moveToPosition -= Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToComeOut + 1);
                    }
                }
            }

            if (Vector3.Distance(moveToPosition, _model.position) < 2 && _model.isAttacking && _model.isLastShotHitTarget == false)
            {
                var vectorFromEnemyToMe = _model.position - target.position;
                var rotatedVectorFromEnemyToMe = Quaternion.Euler(0, 90, 0) * vectorFromEnemyToMe;
                moveToPosition = target.position + rotatedVectorFromEnemyToMe;
            }
        }

        return moveToPosition;
    }

    private UnitModel GetClosestEnemy(IEnumerable<UnitFacade> unitsToIterate)
    {
        UnitModel closest = null;
        foreach (var unit in unitsToIterate)
        {
            if (_model.teamId != unit.UnitModel.teamId)
            {
                var distance = Vector3.Distance(_model.position, unit.UnitModel.position);
                if (closest == null || distance < Vector3.Distance(_model.position, closest.position))
                {
                    closest = unit.UnitModel;
                }
            }
        }
        return closest;
    }

    private IList<UnitModel> GetUnitsTargetedMe(IEnumerable<UnitFacade> unitsToIterate)
    {
        var result = new List<UnitModel>();
        foreach (var unit in unitsToIterate)
        {
            if (unit.UnitModel.attackTarget?.UnitModel == _model)
            {
                result.Add(unit.UnitModel);
            }
        }

        return result;
    }
    public void Dispose()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;

        _coroutinesManager?.StopAllComponentCoroutines();
    }
}
