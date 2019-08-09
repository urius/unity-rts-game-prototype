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
    protected Promise<UnitFacade> hitPromise;


    public IPromise<UnitFacade> Initialize(UnitFacade striker, UnitFacade target, Vector3 from, Vector3 direction, int damage)
    {
        this.target = target.UnitModel;
        this.from = from;
        this.direction = direction;

        hitPromise = new Promise<UnitFacade>();
        return hitPromise.Then(hitUnit => ProcessHit(hitUnit, striker, damage));
    }

    public IPromise<UnitFacade> HitPromise => hitPromise;

    private UnitFacade ProcessHit(UnitFacade hitUnit, UnitFacade striker, int damage)
    {
        if (hitUnit != null)
        {
            if (hitUnit.UnitModel.hp > 0)
            {
                hitUnit.UnitModel.DoDamage(damage);
                if (hitUnit.UnitModel.hp <= 0)
                {
                    _signalBus.Fire(new UnitDestroyedBySignal(hitUnit, striker));
                }
            }
        }

        return hitUnit;
    }
}
