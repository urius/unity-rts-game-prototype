﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShowMoney : MonoBehaviour
{
    [Inject]
    private GameData _gameData;
    [Inject]
    private SignalBus _signalBus;

    private IPlayerData _playerData;
    private Text _moneyText;
    private int _lastValue;

    // Start is called before the first frame update
    private void Awake()
    {
        _playerData = _gameData.GetPlayerData(_gameData.UserTeam);
        _moneyText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        _lastValue = _playerData.money;
        RefreshMoneyValue(_lastValue);

        _signalBus.Subscribe<MoneyChangedSignal>(OnMoneyChanged);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<MoneyChangedSignal>(OnMoneyChanged);
    }

    private void OnMoneyChanged(MoneyChangedSignal signal)
    {
        if (signal.teamId == _playerData.team)
        {
            RefreshMoneyValue(signal.newAmount);
        }
    }

    private void RefreshMoneyValue(int value)
    {
        _moneyText.text = value + "$";
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerData.money != _lastValue)
        {
            _lastValue = _playerData.money;
            //RefreshMoneyValue(_lastValue);
        }
    }
}
