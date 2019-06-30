using Zenject;

public class TankInstaller : MovableUnitInstallerBase
{
    [Inject]
    private UnitsConfig _unitsConfig;

    protected override void InstallExtraBindings()
    {
        base.InstallExtraBindings();

        Container.Bind<IMoveAnimationAdapter>()
            .To<TankCannonMoveAnimationAdapter>()
            .AsSingle();

        Container.Bind<ITurretAnimationAdapter>()
            .To<CommonTurretAnimationAdapter>()
            .AsSingle();
    }

    protected override UnitModel CreateUnitModel()
    {
        var config = _unitsConfig.GetConfigByType(MobileUnitType.Tank);
        return new UnitModel(team, config.hp, config.cost, config.detectRadius);
    }
}