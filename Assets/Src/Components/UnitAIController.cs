using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class UnitAIController : MonoBehaviour
{
    [Inject]
    private UnitsCollectionProvider _unitsCollectionProvider;
    [Inject]
    private UnitModel _model;

    private NavMeshMoveToMouse _moveToMouseScript;
    private NavMeshMoveDecorator _moveToPositionScript;


    void Awake()
    {
        _moveToMouseScript = GetComponent<NavMeshMoveToMouse>();
        _moveToPositionScript = GetComponent<NavMeshMoveDecorator>();
    }
    void Start()
    {
        StartCoroutine(ChooseAttackTargetCoroutine());
        StartCoroutine(ChooseMoveToTargetCoroutine());
    }

    private bool needToProcessMoving => _moveToMouseScript == null || !_moveToMouseScript.isActiveAndEnabled;
    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator ChooseAttackTargetCoroutine()
    {
        while (true)
        {
            if (!enabled)
            {
                yield return new WaitForFixedUpdate();
            }

            var attackTarget = GetClosestAttackableEnemy(_unitsCollectionProvider.units);
            _model.turretTarget = attackTarget;

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ChooseMoveToTargetCoroutine()
    {
        while (true)
        {
            if (!enabled)
            {
                yield return new WaitForFixedUpdate();
            }

            if (needToProcessMoving)
            {
                var target = GetClosestEnemy(_unitsCollectionProvider.units);

                ProcessMovingTo(target);
            }

            yield return new WaitForSeconds(3f);
        }
    }

    private void ProcessMovingTo(UnitModel target)
    {
        var moveToPosition = transform.position;
        if (target != null && target.hp > 0)
        {
            var distanceToClosest = Vector3.Distance(target.transform.position, transform.position);

            if (target.detectRadius < _model.detectRadius)
            {
                //go to the middle between detect radiuses
                var vectorFromEnemyToMe = transform.position - target.transform.position;
                var distanceToCome = target.detectRadius + (_model.detectRadius - target.detectRadius) / 2;

                moveToPosition = target.transform.position + Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToCome);

                //Debug.DrawLine(transform.position + transform.up, moveToPosition, Color.green, 1f);
            }
            else //if (distanceToClosest > _unitAvatar.detectRadius)
            {
                //need to come closer
                var vectorFromEnemyToMe = transform.position - target.transform.position;
                var distanceToCome = _model.detectRadius / 2;
                moveToPosition = target.transform.position + Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToCome);

                //Debug.DrawLine(transform.position, moveToPosition, Color.red, 1f);
            }

            var team = _model.teamId;
            var enemiesExceptTarget = _unitsCollectionProvider.units.FindAll(u => (u.teamId != team) && (u != target));
            var extraEnemiesTargetedToMe = GetUnitsTargetedMe(enemiesExceptTarget);
            if (extraEnemiesTargetedToMe.Count > 0)
            {

                //move out from danger zone
                foreach (var targettingEnemy in extraEnemiesTargetedToMe)
                {
                    var vectorFromEnemyToMe = targettingEnemy.transform.position - transform.position;
                    var distanceToComeOut = targettingEnemy.detectRadius - vectorFromEnemyToMe.magnitude;
                    if (distanceToComeOut > 0)
                    {
                        moveToPosition -= Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToComeOut + 1);
                    }
                }
            }

            if (Vector3.Distance(moveToPosition, transform.position) < 2 && _model.isAttacikng && _model.isLastShotHitTarget == false)
            {
                var vectorFromEnemyToMe = transform.position - target.transform.position;
                var rotatedVectorFromEnemyToMe = Quaternion.Euler(0, 90, 0) * vectorFromEnemyToMe;
                moveToPosition = target.transform.position + rotatedVectorFromEnemyToMe;
            }

            _moveToPositionScript.MoveToPosition(moveToPosition);
        }
    }

    private UnitModel GetClosestEnemy(IEnumerable<UnitModel> unitsToIterate)
    {
        UnitModel closest = null;
        foreach (var unit in unitsToIterate)
        {
            if (_model.teamId != unit.teamId)
            {
                var distance = Vector3.Distance(transform.position, unit.transform.position);
                if (closest == null || distance < Vector3.Distance(transform.position, closest.transform.position))
                {
                    closest = unit;
                }
            }
        }
        return closest;
    }

    private UnitModel GetClosestAttackableEnemy(IEnumerable<UnitModel> unitsToIterate)
    {
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

    private IList<UnitModel> GetUnitsTargetedMe(IEnumerable<UnitModel> unitsToIterate)
    {
        var result = new List<UnitModel>();
        foreach (var unit in unitsToIterate)
        {
            if (unit.turretTarget == _model)
            {
                result.Add(unit);
            }
        }

        return result;
    }
}
