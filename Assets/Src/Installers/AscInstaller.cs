using Zenject;

public class AscInstaller : MovableUnitInstallerBase
{
    [Inject]
    private UnitsConfig _unitsConfig;

    public override void InstallBindings()
    {
        base.InstallBindings();

        Container.Bind<IMoveAnimationAdapter>()
            .To<ACSMoveAnimationAdapter>()
            .AsSingle();

        Container.Bind<ITurretAnimationAdapter>()
            .To<CommonTurretAnimationAdapter>()
            .AsSingle();
    }

    protected override UnitModel CreateUnitModel()
    {
        var config = _unitsConfig.GetConfigByType(MobileUnitType.Robot);
        return new UnitModel(team, config.hp, config.cost, config.detectRadius);
    }
}