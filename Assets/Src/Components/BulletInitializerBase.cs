using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BulletInitializerBase : MonoBehaviour, IBulletInitializer
{
    public async Task<UnitAvatar> Initialize(UnitAvatar striker, UnitAvatar target, Vector3 from, Vector3 direction, int damage)
    {
        var initialTargetHp = target.hp;
        var hitUnit = await InitializeInternal(striker, target, from, direction, damage);
        if (hitUnit != null && initialTargetHp > 0 && target == hitUnit && hitUnit.hp <= 0)
        {
            EventBus.UnitDestroyedBy.Invoke(hitUnit, striker);
        }
        return hitUnit;
    }
    protected abstract Task<UnitAvatar> InitializeInternal(UnitAvatar striker, UnitAvatar target, Vector3 from, Vector3 direction, int damage);
}
