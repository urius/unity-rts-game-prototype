using System;
using System.Collections;
using UnityEngine;
using Zenject;

[Serializable]
public class WeaponConfig
{
    public Animator animator;
    public Transform firePoint;
    public UnityEngine.Object BulletPrefab;
    public float reloadTimeSeconds = 2.2f;
    public float delayBeforeFirstShotSeconds = 1.1f;
    public int damagePerShot = 1;
}

public class UnitTurretController : IInitializable, ILateTickable
{
    [Inject]
    private BulletFactory _bulletFactory;
    [Inject]
    private CoroutinesManager _coroutinesManager;
    [Inject]
    private UnitModel _model;
    [Inject]
    private UnitFacade _facade;
    [Inject]
    private ITurretAnimationAdapter _turretAnimationAdapter;


    private Settings _settings;
    private UnitModel _target => _model.attackTarget?.UnitModel;
    private Quaternion _lastRotation;

    public UnitTurretController(Settings settings)
    {
        _settings = settings;
    }

    private float turnSpeed => _settings.turnSpeedPerSecond * Time.deltaTime;

    public void Initialize()
    {
        _lastRotation = _settings.turret.transform.rotation;

        foreach (var item in _settings.weapons)
        {
            _coroutinesManager.StartCoroutine(FireOnTargetCoroutine(item));
        }
    }

    public bool canAttack => _target != null && _target.isAlive
        && (Vector3.Distance(_target.position, _model.position) <= _model.detectRadius);

    public void LateTick()
    {
        var turret = _settings.turret;
        if (_model.isAlive)
        {
            if (_target != null && _target.isAlive)
            {
                var targetLookAtPoint = _target.position;
                targetLookAtPoint.y = turret.transform.position.y;

                var targetRotation = Quaternion.LookRotation(targetLookAtPoint - turret.transform.position, turret.transform.up);
                turret.transform.rotation = Quaternion.RotateTowards(_lastRotation, targetRotation, turnSpeed);
            }
            else
            {
                turret.transform.rotation = Quaternion.RotateTowards(_lastRotation, turret.transform.rotation, turnSpeed);
            }
        }
        else if (_settings.freezeRotationAfterDestroy)
        {
            turret.transform.rotation = _lastRotation;
        }

        _lastRotation = turret.transform.rotation;
    }

    private IEnumerator FireOnTargetCoroutine(WeaponConfig weapon)
    {
        var turret = _settings.turret;
        var needToDelay = true;
        while (true)
        {
            if (_model.isAlive && canAttack)
            {
                _model.isAttacking = true;
                if (needToDelay)
                {
                    needToDelay = false;
                    yield return new WaitForSeconds(weapon.delayBeforeFirstShotSeconds);
                }

                var targetLookAtPoint = _target.position;
                targetLookAtPoint.y = turret.transform.position.y;

                var fireDirection = _target.position - turret.transform.position;
                var projectedfireDirection = Vector3.ProjectOnPlane(fireDirection, Vector3.up);

                if (Quaternion.Angle(turret.rotation, Quaternion.LookRotation(projectedfireDirection, turret.transform.up)) <= 0.5f)
                {
                    _turretAnimationAdapter.Fire(weapon.animator);
                    Fire(weapon, fireDirection);

                    yield return new WaitForSeconds(weapon.reloadTimeSeconds);
                }
            }
            else
            {
                _model.isAttacking = false;
                needToDelay = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void Fire(WeaponConfig weapon, Vector3 fireDirection)
    {
        _bulletFactory.Create(_facade, _model.attackTarget, weapon, fireDirection)
                        .HitPromise
                        .Then(hitUnit => _model.isLastShotHitTarget = hitUnit == _model.attackTarget);
    }

    [Serializable]
    public class Settings
    {
        public int turnSpeedPerSecond = 100;
        public bool freezeRotationAfterDestroy = false;
        public Transform turret;
        public WeaponConfig[] weapons;
    }
}
