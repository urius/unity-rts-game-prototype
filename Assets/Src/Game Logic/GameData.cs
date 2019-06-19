using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int team;
    public int money;
}
public class GameData : MonoBehaviour
{
    public int userTeam = 0;
    // Start is called before the first frame update
    [SerializeField]
    private PlayerData[] playersData;
    void Start()
    {
    }

    public PlayerData GetPlayerData(int team)
    {
        return Array.Find(playersData, d => d.team == team);
    }

    public bool TryChangePlayerMoney(int team, int deltaMoney)
    {
        if (playersData[team].money + deltaMoney >= 0)
        {
            playersData[team].money += deltaMoney;
            return true;
        }
        return false;
    }
}
