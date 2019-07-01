using System;
using UnityEngine;
using Zenject;

public class UnitFactoryController : ITickable
{
    [Inject]
    private GameData _gameData;
    [Inject]
    private UnitModel _unitModel;
    [Inject]
    private UnitFactoryModel _factoryModel;

    [Inject]
    private UnitsConfig _unitsConfig;
    [Inject]
    private UnitFactory _unitFactory;

    private Settings _settings;

    public UnitFactoryController(Settings settings)
    {
        _settings = settings;
    }

    public void Tick()
    {
        if (!_unitModel.isAlive)
        {
            return;
        }

        if (_factoryModel.isBuilding)
        {
            var buildingUnitConfig = _unitsConfig.GetConfigByType(_factoryModel.currentBuildingUnitType);
            var buildProgress = _factoryModel.AdvanceBuildProgress(Time.deltaTime / buildingUnitConfig.buildTimeSeconds);

            if (buildProgress >= 1)
            {
                var spawnPointTransform = _settings.spawnPoint.transform;
                var unit = _unitFactory.Create(buildingUnitConfig.typeId, _unitModel.teamId, spawnPointTransform);

                var spawnTargetPoint = spawnPointTransform.position + (_factoryModel.GetUnitsCountInBuildQueue() + 1) * (spawnPointTransform.position - _unitModel.position);
                unit.UnitModel.destinationPoint = spawnTargetPoint;
            }
        }
    }

    [Serializable]
    public class Settings
    {
        public Transform spawnPoint;
    }
}
