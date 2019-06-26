using System;
using UnityEngine;
using Zenject;

public interface IPlayerData
{
    int team { get; }
    int money { get; }
}
[Serializable]
public class PlayerData : IPlayerData
{
    [SerializeField]
    private int _team;
    public int team { get { return _team; } set { _team = value; } }
    [SerializeField]
    private int _money;
    public int money { get { return _money; } set { _money = value; } }
}

[Serializable]
public class GameData : IInitializable, IDisposable
{
    [Inject]
    private SignalBus _signalBus;


    [SerializeField]
    private int _userTeam = 0;
    // Start is called before the first frame update
    [SerializeField]
    private PlayerData[] playersData;

    public int UserTeam => _userTeam;

    public void Initialize()
    {
        Debug.Log("new _signalBus: " + (_signalBus != null));
    }

    public IPlayerData GetPlayerData(int team)
    {
        return Array.Find(playersData, d => d.team == team);
    }

    public bool TryChangePlayerMoney(int team, int deltaMoney)
    {
        if (playersData[team].money + deltaMoney >= 0)
        {
            playersData[team].money += deltaMoney;

            _signalBus.Fire(new MoneyChangedSignal() { teamId = team, newAmount = playersData[team].money });

            return true;
        }
        return false;
    }
    public void Dispose()
    {
        Debug.Log("new _signalBus: " + (_signalBus != null));
    }
}
