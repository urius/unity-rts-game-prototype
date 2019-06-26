using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class RobotsFactoryController : MonoBehaviour
{
    private class BuildingUnit
    {
        public float timeToBuildLeft;
        public MobileUnitConfig unit;
    }


    [Inject]
    private DiContainer _container;
    [Inject]
    private GameData _gameData;
    [Inject]
    private UnitModel _model;

    [Inject]
    private UnitsConfig _unitsConfig;
    [Inject]
    private UnitFactory _unitFactory;


    public Action<MobileUnitType, float> UpdateBuildInfo = delegate { };

    [SerializeField]
    private Transform _spawnPoint;
    private readonly List<BuildingUnit> _buildQueue = new List<BuildingUnit>();

    public bool isBuilding => _buildQueue.Count > 0;

    public void AddToBuildQueue(MobileUnitType unitType)
    {
        var buildingUnitConfig = _unitsConfig.GetConfigByType(unitType);
        if (_gameData.TryChangePlayerMoney(_model.teamId, -buildingUnitConfig.cost))
        {
            _buildQueue.Add(new BuildingUnit()
            {
                timeToBuildLeft = buildingUnitConfig.buildTimeSeconds,
                unit = buildingUnitConfig
            });
        }
    }

    public int GetUnitsCountInBuildQueue(MobileUnitType type)
    {
        var targetUnit = _unitsConfig.GetConfigByType(type);
        return _buildQueue.FindAll(u => u.unit.typeId == targetUnit.typeId).Count;
    }


    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            var buildingUnitData = _buildQueue[0];
            buildingUnitData.timeToBuildLeft -= Time.deltaTime;
            if (buildingUnitData.timeToBuildLeft <= 0)
            {
                buildingUnitData.timeToBuildLeft = 0;
            }

            var buildProgress = (buildingUnitData.unit.buildTimeSeconds - buildingUnitData.timeToBuildLeft) / buildingUnitData.unit.buildTimeSeconds;

            if (buildProgress >= 1)
            {
                _buildQueue.RemoveAt(0);

                var unit = _unitFactory.Create(buildingUnitData.unit.typeId, _model.teamId, _spawnPoint.transform);

                var spawnTargetPoint = _spawnPoint.transform.position + _spawnPoint.transform.position - transform.position;
                unit.GetComponent<NavMeshAgent>().SetDestination(spawnTargetPoint);
            }

            UpdateBuildInfo(buildingUnitData.unit.typeId, buildProgress);
        }
    }
}
