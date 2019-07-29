using RSG;
using UnityEngine;
using Zenject;

public abstract class BulletBase : MonoBehaviour, IBullet
{
    [Inject]
    private SignalBus _signalBus;


    protected UnitModel target;
    protected Vector3 from;
    protected Vector3 direction;
    protected Promise<UnitModel> hitPromise;


    public IPromise<UnitModel> Initialize(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage)
    {
        this.target = target;
        this.from = from;
        this.direction = direction;

        hitPromise = new Promise<UnitModel>();
        return hitPromise.Then(hitUnit => ProcessHit(hitUnit, striker, damage));
    }

    public IPromise<UnitModel> HitPromise => hitPromise;

    private UnitModel ProcessHit(UnitModel hitUnit, UnitModel striker, int damage)
    {
        if (hitUnit != null)
        {
            if (hitUnit.hp > 0)
            {
                hitUnit.DoDamage(damage);
                if (hitUnit.hp <= 0)
                {
                    _signalBus.Fire(new UnitDestroyedBySignal(hitUnit, striker));
                }
            }
        }

        return hitUnit;
    }
}
