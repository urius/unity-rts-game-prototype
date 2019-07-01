using System;
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

    public void Initialize()
    {
        _coroutinesManager.StartCoroutine(ChooseTargetToMoveCoroutine());
        _model.UnitDestroyed += OnUnitDestroyed;
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
            //var target = GetClosestEnemy(_unitsCollectionProvider.units);
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
            var enemiesExceptTarget = UnitsCollectionProvider._units.FindAll(u => (u.teamId != team) && (u != target));
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

    private UnitModel GetClosestEnemy(IEnumerable<UnitModel> unitsToIterate)
    {
        UnitModel closest = null;
        foreach (var unit in unitsToIterate)
        {
            if (_model.teamId != unit.teamId)
            {
                var distance = Vector3.Distance(_model.position, unit.position);
                if (closest == null || distance < Vector3.Distance(_model.position, closest.position))
                {
                    closest = unit;
                }
            }
        }
        return closest;
    }

    private IList<UnitModel> GetUnitsTargetedMe(IEnumerable<UnitModel> unitsToIterate)
    {
        var result = new List<UnitModel>();
        foreach (var unit in unitsToIterate)
        {
            if (unit.attackTarget == _model)
            {
                result.Add(unit);
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
