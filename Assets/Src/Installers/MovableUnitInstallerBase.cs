using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class MovableUnitInstallerBase : UnitInstallerBase
{
    [Inject]
    private GameData _gameData;

    [SerializeField]
    private UnitNavMeshMoveController.Settings _movingSettings;

    [SerializeField]
    private UnitTurretController.Settings _turretSettings;

    protected override void InstallExtraBindings()
    {
        Container.BindInstance(gameObject.GetComponent<Animator>()).AsSingle();
        Container.BindInstance(gameObject.GetComponent<SelectableDestroyableView>()).AsSingle();
        Container.BindInstance(gameObject.GetComponent<NavMeshAgent>()).AsSingle();

        Container.BindInterfacesAndSelfTo<UnitAIAttackController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnitNavMeshMoveController>().AsSingle().WithArguments(_movingSettings).NonLazy();
        Container.BindInterfacesAndSelfTo<UnitTurretController>().AsSingle().WithArguments(_turretSettings).NonLazy();


        if (team == _gameData.UserTeam)
        {
            Container.BindInterfacesAndSelfTo<UnitMoveByUserController>().AsSingle().NonLazy();
        }
        else
        {
            Container.BindInterfacesAndSelfTo<UnitAIMoveController>().AsSingle().NonLazy();
        }
    }
}
