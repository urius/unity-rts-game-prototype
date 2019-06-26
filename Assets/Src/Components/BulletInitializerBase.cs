using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public abstract class BulletInitializerBase : MonoBehaviour, IBulletInitializer
{
    [Inject]
    private SignalBus _signalBus;
    public async Task<UnitModel> Initialize(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage)
    {
        var initialTargetHp = target.hp;
        var hitUnit = await InitializeInternal(striker, target, from, direction, damage);
        if (hitUnit != null && initialTargetHp > 0 && target == hitUnit && hitUnit.hp <= 0)
        {
            _signalBus.Fire(new UnitDestroyedBySignal(hitUnit, striker));
        }
        return hitUnit;
    }
    protected abstract Task<UnitModel> InitializeInternal(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage);
}
