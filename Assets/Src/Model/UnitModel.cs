using System;
using UnityEngine;
using Zenject;

public class UnitModel
{
    private static int lastId = 0;
    private int id = lastId;
    public event Action<int> HealthUpdated = delegate { };
    public event Action UnitDestroyed = delegate { };
    public event Action<Vector3> DestinationChanged = delegate { };
    public event Action<bool> SelectionChanged = delegate { };


    [Inject]
    public readonly Transform transform;

    public readonly int teamId;
    public readonly int cost;
    public readonly int maxHp;
    public readonly int detectRadius;

    public bool isAttacikng = false;
    public bool isLastShotHitTarget = false;
    public UnitModel attackTarget = null;
    private Vector3 _destinationPoint;
    private bool _isSelected;

    public UnitModel(int teamId, int hp, int cost, int detectRadius)
    {
        id = ++UnitModel.lastId;

        this.teamId = teamId;
        this.hp = hp;
        maxHp = hp;
        this.cost = cost;
        this.detectRadius = detectRadius;

        _isSelected = false;
    }

    public bool isAlive => hp > 0;
    public Vector3 destinationPoint
    {
        get { return _destinationPoint; }
        set
        {
            _destinationPoint = value;
            DestinationChanged(value);
        }
    }
    public bool isSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            SelectionChanged(_isSelected);
        }
    }

    public int hp { get; private set; }

    public void DoDamage(int points)
    {
        if (hp <= 0)
        {
            return;
        }
        hp = Mathf.Max(0, hp - points);

        HealthUpdated(hp);
        if (hp <= 0)
        {
            UnitDestroyed();
        }
    }
}
