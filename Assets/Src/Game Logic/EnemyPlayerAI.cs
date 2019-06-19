using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerAI : MonoBehaviour
{
    [SerializeField]
    private int team;

    [SerializeField]
    private GameData gameData;
    private RobotsFactoryController _robotsFactory;

    private PlayerData _playerData;
    void Start()
    {
        var robotsFactories = FindObjectsOfType<RobotsFactoryController>();
        _robotsFactory = Array.Find(robotsFactories, f => f.GetComponent<UnitAvatar>().team == team);

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

            var availableUnitIndices = new List<int>();
            for (var i = 0; i < _robotsFactory.BuildableUnits.Length; i++)
            {
                if (_robotsFactory.BuildableUnits[i].cost <= _playerData.money)
                {
                    availableUnitIndices.Add(i);
                }
            }

            if (availableUnitIndices.Count > 0)
            {
                var orderUnitIndex = availableUnitIndices[UnityEngine.Random.Range(0, availableUnitIndices.Count)];
                _robotsFactory.AddToBuildQueue(orderUnitIndex);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
