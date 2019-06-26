using UnityEngine;
using Zenject;

public class AscInstaller : UnitInstallerBase
{
    [Inject]
    private UnitsConfig _unitsConfig;
    
    public override void InstallBindings()
    {
        base.InstallBindings();
        
        Container.BindInstance<Animator>(gameObject.GetComponent<Animator>());

        Container.Bind<IMoveAnimationAdapter>()
            .To<ACSMoveAnimationAdapter>()
            .AsSingle();
    }

    protected override UnitModel CreateUnitModel()
    {
        var config = _unitsConfig.GetConfigByType(MobileUnitType.Robot);
        return new UnitModel(team, config.hp, config.cost, config.detectRadius);
    }
}