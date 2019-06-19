using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAIController : MonoBehaviour
{
    private UnitAvatar _unitAvatar;
    private NavMeshMoveToMouse _moveToMouseScript;
    private TurretController _turretController;
    private NavMeshMoveDecorator _moveToPositionScript;


    void Awake()
    {
        _unitAvatar = GetComponent<UnitAvatar>();
        _moveToMouseScript = GetComponent<NavMeshMoveToMouse>();
        _turretController = GetComponent<TurretController>();
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

            var attackTarget = GetClosestAttackableEnemy(UnitAvatar.AllUnits);
            _turretController.target = attackTarget?.transform;

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
                var target = GetClosestEnemy(UnitAvatar.AllUnits);

                ProcessMovingTo(target);
            }

            yield return new WaitForSeconds(3f);
        }
    }

    private void ProcessMovingTo(UnitAvatar target)
    {
        var moveToPosition = transform.position;
        if (target != null && target.hp > 0)
        {
            var distanceToClosest = Vector3.Distance(target.transform.position, transform.position);

            if (target.detectRadius < _unitAvatar.detectRadius)
            {
                //go to the middle between detect radiuses
                var vectorFromEnemyToMe = transform.position - target.transform.position;
                var distanceToCome = target.detectRadius + (_unitAvatar.detectRadius - target.detectRadius) / 2;

                moveToPosition = target.transform.position + Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToCome);

                //Debug.DrawLine(transform.position + transform.up, moveToPosition, Color.green, 1f);
            }
            else //if (distanceToClosest > _unitAvatar.detectRadius)
            {
                //need to come closer
                var vectorFromEnemyToMe = transform.position - target.transform.position;
                var distanceToCome = _unitAvatar.detectRadius / 2;
                moveToPosition = target.transform.position + Vector3.ClampMagnitude(vectorFromEnemyToMe, distanceToCome);

                //Debug.DrawLine(transform.position, moveToPosition, Color.red, 1f);
            }

            var team = _unitAvatar.team;
            var enemiesExceptTarget = UnitAvatar.AllUnits.FindAll(u => (u.team != team) && (u != target));
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

            if (Vector3.Distance(moveToPosition, transform.position) < 2 && _unitAvatar.isAttacikng && _unitAvatar.isLastShotHitTarget == false)
            {
                var vectorFromEnemyToMe = transform.position - target.transform.position;
                var rotatedVectorFromEnemyToMe = Quaternion.Euler(0, 90, 0) * vectorFromEnemyToMe;
                moveToPosition = target.transform.position + rotatedVectorFromEnemyToMe;
            }

            _moveToPositionScript.MoveToPosition(moveToPosition);
        }
    }

    private UnitAvatar GetClosestEnemy(IEnumerable<UnitAvatar> unitsToIterate)
    {
        UnitAvatar closest = null;
        foreach (var unit in unitsToIterate)
        {
            if (_unitAvatar.team != unit.team)
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

    private UnitAvatar GetClosestAttackableEnemy(IEnumerable<UnitAvatar> unitsToIterate)
    {
        UnitAvatar closest = null;
        foreach (var unit in unitsToIterate)
        {
            if (_unitAvatar.team != unit.team)
            {
                var distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance <= _unitAvatar.detectRadius)
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

    private IList<UnitAvatar> GetUnitsTargetedMe(IEnumerable<UnitAvatar> unitsToIterate)
    {
        var result = new List<UnitAvatar>();
        foreach (var unit in unitsToIterate)
        {
            if (unit.turretTarget == gameObject)
            {
                result.Add(unit);
            }
        }

        return result;
    }
}
