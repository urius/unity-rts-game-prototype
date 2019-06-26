using System;
using UnityEngine;
using Zenject;

public class UnitModel
{
    public event Action<int> HealthUpdated = delegate {};

    public readonly int teamId;
    public readonly int cost;
    public readonly int maxHp;
    public readonly int detectRadius;
    [Inject]
    public readonly Transform transform;

    public bool isAttacikng = false;
    public bool isLastShotHitTarget = false;
    public UnitModel turretTarget = null;

    public UnitModel(int teamId, int hp, int cost, int detectRadius)
    {
        this.teamId = teamId;
        this.hp = hp;
        maxHp = hp;
        this.cost = cost;
        this.detectRadius = detectRadius;
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

        //UpdatePercent(); // TODO
    }
}
