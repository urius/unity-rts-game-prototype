using System;
using System.Collections.Generic;

public interface IReadonlyFactoryModel {
    event Action<MobileUnitType, float> BuildProgressUpdated;
    int GetUnitsCountInBuildQueue(MobileUnitType type);
}

public class UnitFactoryModel : IReadonlyFactoryModel
{
    public event Action<MobileUnitType, float> BuildProgressUpdated = delegate {};

    private readonly List<BuildingUnit> _buildQueue = new List<BuildingUnit>();


    public bool isBuilding => _buildQueue.Count > 0;
    public MobileUnitType currentBuildingUnitType => isBuilding ? _buildQueue[0].unitType : MobileUnitType.None;

    public void AddToBuildQueue(MobileUnitType unitType)
    {
        _buildQueue.Add(new BuildingUnit()
        {
            buildProgress = 0f,
            unitType = unitType
        });
    }

    public int GetUnitsCountInBuildQueue()
    {
        return _buildQueue.Count;
    }

    public int GetUnitsCountInBuildQueue(MobileUnitType type)
    {
        return _buildQueue.FindAll(u => u.unitType == type).Count;
    }

    private class BuildingUnit
    {
        public float buildProgress;
        public MobileUnitType unitType;
    }

    public float AdvanceBuildProgress(float progress)
    {
        var buildingUnit = _buildQueue[0];
        buildingUnit.buildProgress = Math.Min(buildingUnit.buildProgress + progress, 1);
        if (buildingUnit.buildProgress >= 1)
        {
            _buildQueue.RemoveAt(0);
        }

        BuildProgressUpdated(buildingUnit.unitType, buildingUnit.buildProgress);

        return buildingUnit.buildProgress;
    }
}
