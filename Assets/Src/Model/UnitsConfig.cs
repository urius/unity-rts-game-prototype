using System;
using UnityEngine;

[Serializable]
public class UnitsConfig
{
    public MobileUnitConfig[] mobileUnits;

    public MobileUnitConfig GetConfigByType(MobileUnitType type)
    {
        return Array.Find(mobileUnits, u => u.typeId == type);
    }
}

public enum MobileUnitType
{
    Tank = 1,
    Robot = 2,
}
[Serializable]
public class MobileUnitConfig
{
    public MobileUnitType typeId;
    public int cost;
    public float buildTimeSeconds;
    public UnityEngine.Object prefab;
    public int hp;
    public int detectRadius;
}
