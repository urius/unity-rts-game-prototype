using UnityEngine;
using Zenject;

public class BulletFactory
{
    [Inject]
    private DiContainer _container;
    public IBullet Create(UnitModel attacker, UnitModel target, WeaponConfig weapon, Vector3 fireDirection)
    {
        var bullet = GameObject.Instantiate(weapon.BulletPrefab) as GameObject;
        _container.InjectGameObject(bullet);
        var bulletFireComponent = bullet.GetComponent<BulletBase>();

        bulletFireComponent.Initialize(attacker, target, weapon.firePoint.position, fireDirection, weapon.damagePerShot);
        
        return bulletFireComponent;
    }
}
