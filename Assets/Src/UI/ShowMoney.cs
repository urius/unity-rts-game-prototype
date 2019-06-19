using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMoney : MonoBehaviour
{
    [SerializeField]
    private int _team;
    [SerializeField]
    private GameData _gameData;
    private PlayerData _playerData;
    private Text _moneyText;
    private int _lastValue;

    // Start is called before the first frame update
    void Awake()
    {
        _playerData = _gameData.GetPlayerData(_team);
        _moneyText = GetComponent<Text>();
    }
    void Start()
    {
        _lastValue = _playerData.money;
        RefreshMoneyValue(_lastValue);
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
            RefreshMoneyValue(_lastValue);
        }
    }
}
