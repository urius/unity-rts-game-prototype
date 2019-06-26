using System;
using System.Collections;
using UnityEngine;
using Zenject;

[Serializable]
public class WeaponData
{
    public Transform firePoint;
    public UnityEngine.Object BulletPrefab;
    public float reloadTimeSeconds = 2.2f;
    public float delayBeforeFirstShotSeconds = 1.1f;
    public int damagePerShot = 1;
}
//[RequireComponent(typeof(UnitAvatar))]
public class TurretController : MonoBehaviour
{
    [Inject]
    private DiContainer _container;
    [Inject]
    private UnitModel _model;
    public Transform turret;
    public WeaponData[] weapons;
    public int detectRadius = 50;

    private UnitModel _target => _model.turretTarget;
    private Quaternion _lastRotation;

    private float turnSpeed => 100 * Time.deltaTime;

    private ITurretAnimationAdapter _turretAnimationAdapter;

    private UnityEngine.Object _bulletTracePrefab;
    private UnityEngine.Object _sparksPrefab;
    private bool _isLastShotHitTarget = false;
    private bool _isAttacking = false;

    void Awake()
    {
        _lastRotation = transform.rotation;

        _bulletTracePrefab = Resources.Load("BulletTrace");
        _sparksPrefab = Resources.Load("Sparks");

        _turretAnimationAdapter = GetComponent<ITurretAnimationAdapter>();
    }

    void Start()
    {
        foreach (WeaponData item in weapons)
        {
            StartCoroutine(FireOnTargetCoroutine(item));
        }
    }
    public bool isAttacking => _isAttacking;

    public bool isLastShotHitTarget => _isLastShotHitTarget;

    public bool canAttack => _target != null && (Vector3.Distance(_target.transform.position, transform.position) <= detectRadius);

    void LateUpdate()
    {
        if (_target != null)
        {
            var targetLookAtPoint = _target.transform.position;
            targetLookAtPoint.y = turret.transform.position.y;

            var targetRotation = Quaternion.LookRotation(targetLookAtPoint - turret.transform.position, turret.transform.up);
            turret.transform.rotation = Quaternion.RotateTowards(_lastRotation, targetRotation, turnSpeed);
        }
        else
        {
            turret.transform.rotation = Quaternion.RotateTowards(_lastRotation, turret.transform.rotation, turnSpeed);
        }

        _lastRotation = turret.transform.rotation;
    }

    private IEnumerator FireOnTargetCoroutine(WeaponData weapon)
    {
        var needToDelay = true;
        while (true)
        {
            if (enabled && canAttack)
            {
                _isAttacking = true;
                if (needToDelay)
                {
                    needToDelay = false;
                    yield return new WaitForSeconds(weapon.delayBeforeFirstShotSeconds);
                }

                var targetLookAtPoint = _target.transform.position;
                targetLookAtPoint.y = turret.transform.position.y;

                var fireDirection = _target.transform.position - turret.transform.position;
                var projectedfireDirection = Vector3.ProjectOnPlane(fireDirection, gameObject.transform.up);

                if (Quaternion.Angle(turret.rotation, Quaternion.LookRotation(projectedfireDirection, turret.transform.up)) <= 0.5f)
                {
                    _turretAnimationAdapter.Fire(Array.IndexOf(weapons, weapon));
                    Fire(weapon, fireDirection);

                    yield return new WaitForSeconds(weapon.reloadTimeSeconds);
                }
            }
            else
            {
                _isAttacking = false;
                needToDelay = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private async void Fire(WeaponData weapon, Vector3 fireDirection)
    {
        var bullet = _container.InstantiatePrefab(weapon.BulletPrefab);
        var bulletFireComponent = bullet.GetComponent<IBulletInitializer>();

        _isLastShotHitTarget = await bulletFireComponent.Initialize(_model, _model.turretTarget, weapon.firePoint.position, fireDirection, weapon.damagePerShot) != null;
    }
}
