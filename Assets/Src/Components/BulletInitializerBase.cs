using RSG;
using UnityEngine;
using Zenject;

public abstract class BulletInitializerBase : MonoBehaviour, IBulletInitializer
{
    [Inject]
    private SignalBus _signalBus;
    public IPromise<UnitModel> Initialize(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage)
    {
        return InitializeInternal(striker, target, from, direction, damage)
                                .Then(hitUnit => ProcessHit(hitUnit, striker, damage));
    }

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
    protected abstract IPromise<UnitModel> InitializeInternal(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage);
}
