using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefundForDestroyLogic : MonoBehaviour
{
    private GameData _gameData;

    // Start is called before the first frame update
    void Awake()
    {
        _gameData = GetComponent<GameData>();
    }
    void Start()
    {
        EventBus.UnitDestroyedBy += OnUnitDestroyed;
    }

    void Stop() {
        EventBus.UnitDestroyedBy -= OnUnitDestroyed;
    }

    private void OnUnitDestroyed(UnitAvatar destroyed, UnitAvatar killer)
    {
        _gameData.TryChangePlayerMoney(killer.team, destroyed.cost / 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
