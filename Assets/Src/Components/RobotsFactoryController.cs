using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

[Serializable]
public class BuildableUnit
{
    public int cost = 1;
    public float buildTimeSeconds = 1;
    public UnityEngine.Object prefab;
}

public class RobotsFactoryController : MonoBehaviour
{
    private class BuildingUnit
    {
        public int index;
        public float timeToBuildLeft;
        public BuildableUnit unit;
    }

    public Action<int, float> UpdateBuildInfo = delegate { };

    [SerializeField]
    private BuildableUnit[] _buildableUnits;
    [SerializeField]
    private Transform _spawnPoint;

    private readonly List<BuildingUnit> _buildQueue = new List<BuildingUnit>();
    private UnitAvatar _unitAvatar;
    private GameData _gameData;

    [Inject] 
    private DiContainer _container;

    void Awake()
    {
        _unitAvatar = GetComponent<UnitAvatar>();

        _gameData = FindObjectOfType<GameData>();
    }
    void Start()
    {
    }

    public BuildableUnit[] BuildableUnits => _buildableUnits;

    public bool isBuilding => _buildQueue.Count > 0;

    public void AddToBuildQueue(int unitIndex)
    {
        var buildungUnit = _buildableUnits[unitIndex];
        if (_gameData.TryChangePlayerMoney(_unitAvatar.team, -buildungUnit.cost))
        {
            _buildQueue.Add(new BuildingUnit()
            {
                index = unitIndex,
                timeToBuildLeft = buildungUnit.buildTimeSeconds,
                unit = buildungUnit
            });
        }
    }

    public int GetUnitsCountInBuildQueue(int index)
    {
        var targetUnit = _buildableUnits[index];
        return _buildQueue.FindAll(u => u.unit == targetUnit).Count;
    }

    public BuildableUnit GetUnitInfo(int index)
    {
        return _buildableUnits[index];
    }


    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            var buildingUnit = _buildQueue[0];
            buildingUnit.timeToBuildLeft -= Time.deltaTime;
            if (buildingUnit.timeToBuildLeft <= 0)
            {
                buildingUnit.timeToBuildLeft = 0;
            }

            var buildProgress = (buildingUnit.unit.buildTimeSeconds - buildingUnit.timeToBuildLeft) / buildingUnit.unit.buildTimeSeconds;


            if (buildProgress >= 1)
            {
                _buildQueue.RemoveAt(0);

                var unit = _container.InstantiatePrefab(buildingUnit.unit.prefab, _spawnPoint.transform) as GameObject;
                var unitAvatar = unit.GetComponent<UnitAvatar>();
                unitAvatar.team = _unitAvatar.team;
                unitAvatar.cost = buildingUnit.unit.cost;
                if (_gameData.userTeam == _unitAvatar.team)
                {
                    unitAvatar.GetComponent<NavMeshMoveToMouse>().enabled = true;
                    unitAvatar.GetComponent<Selectable>().enabled = true;
                }

                var spawnTargetPoint = _spawnPoint.transform.position + _spawnPoint.transform.position - transform.position;
                unit.GetComponent<NavMeshAgent>().SetDestination(spawnTargetPoint);
            }

            UpdateBuildInfo(buildingUnit.index, buildProgress);
        }
    }
}
