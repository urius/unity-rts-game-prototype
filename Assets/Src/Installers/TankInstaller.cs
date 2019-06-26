using UnityEngine;
using Zenject;

public class TankInstaller : UnitInstallerBase
{
    [Inject]
    private UnitsConfig _unitsConfig;

    public override void InstallBindings()
    {
        base.InstallBindings();

        Container.BindInstance(gameObject.GetComponent<Animator>());

        Container.Bind<IMoveAnimationAdapter>()
            .To<TankCannonMoveAnimationAdapter>()
            .AsSingle();
    }

    protected override UnitModel CreateUnitModel()
    {
        var config = _unitsConfig.GetConfigByType(MobileUnitType.Tank);
        return new UnitModel(team, config.hp, config.cost, config.detectRadius);
    }
}