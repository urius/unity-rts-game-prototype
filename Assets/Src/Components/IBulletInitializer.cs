using RSG;
using UnityEngine;

public interface IBulletInitializer
{
    IPromise<UnitModel> Initialize(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage);
}
