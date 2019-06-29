using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPlayerAI : MonoBehaviour
{
    [Inject]
    private GameData gameData;
    [Inject]
    private UnitsConfig _unitsConfig;

    [SerializeField]
    private int team;
    private UnitFactoryModel _unitFactoryModel;
    private IPlayerData _playerData;
    void Start()
    {
        var robotsFactories = FindObjectsOfType<UnitFactoryFacade>();
        _unitFactoryModel = Array.Find(robotsFactories, f => f.UnitModel.teamId == team).UnitFactoryModel;

        _playerData = gameData.GetPlayerData(team);

        StartCoroutine(BuilUnitsLogicCorotine());
    }

    private IEnumerator BuilUnitsLogicCorotine()
    {
        while (true)
        {
            if (!enabled)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            var canBeBuiltUnits = new List<MobileUnitType>();
            for (var i = 0; i < _unitsConfig.mobileUnits.Length; i++)
            {
                if (_unitsConfig.mobileUnits[i].cost <= _playerData.money)
                {
                    canBeBuiltUnits.Add(_unitsConfig.mobileUnits[i].typeId);
                }
            }

            if (canBeBuiltUnits.Count > 0)
            {
                var orderUnitType = canBeBuiltUnits[UnityEngine.Random.Range(0, canBeBuiltUnits.Count)];
                _unitFactoryModel.AddUnitRequest(orderUnitType);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
