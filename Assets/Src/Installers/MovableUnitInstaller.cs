using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MovableUnitInstaller : UnitInstallerBase
{
    [Inject]
    private GameData _gameData;
    [Inject]
    private UnitsConfig _unitsConfig;

    [SerializeField]
    private UnitNavMeshMoveController.Settings _movingSettings;

    [SerializeField]
    private UnitTurretController.Settings _turretSettings;
    
    [SerializeField]
    private MobileUnitType _unitType;

    protected override void InstallExtraBindings()
    {
        Container.BindInstance(gameObject.GetComponent<Animator>()).AsSingle();
        Container.BindInstance(gameObject.GetComponent<SelectableView>()).AsSingle();
        Container.BindInstance(gameObject.GetComponent<NavMeshAgent>()).AsSingle();

        Container.BindInterfacesAndSelfTo<UnitAIAttackController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnitNavMeshMoveController>().AsSingle().WithArguments(_movingSettings).NonLazy();
        Container.BindInterfacesAndSelfTo<UnitTurretController>().AsSingle().WithArguments(_turretSettings).NonLazy();


        if (parameters.teamId == _gameData.UserTeam)
        {
            Container.BindInterfacesAndSelfTo<UnitMoveByUserController>().AsSingle().NonLazy();
        }
        else
        {
            Container.BindInterfacesAndSelfTo<UnitAIMoveController>().AsSingle().NonLazy();
        }

        Container.Bind<ITurretAnimationAdapter>()
            .To<CommonTurretAnimationAdapter>()
            .AsSingle();
    }

    protected override UnitModel CreateUnitModel()
    {
        var config = _unitsConfig.GetConfigByType(_unitType);
        return new UnitModel(parameters.teamId, config.hp, config.cost, config.detectRadius);
    }
}
