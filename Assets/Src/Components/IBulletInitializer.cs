using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IBulletInitializer
{
    Task<UnitModel> Initialize(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage);
}
