using UnityEngine;
using Zenject;

public abstract class UnitInstallerBase : MonoInstaller
{
    [SerializeField]
    protected int team;

    [SerializeField]
    private GameObject _explosionPrefab;

    public void SetupTeam(int team)
    {
        this.team = team;
    }

    public override void InstallBindings()
    {
        Container.BindInstance<UnitFacade>(transform.GetComponent<UnitFacade>()).AsSingle();

        var model = CreateUnitModel();
        Container.BindInstance<UnitModel>(model).AsSingle();

        Container.BindInstance<Transform>(transform).AsSingle();
        Container.BindInstance<GameObject>(_explosionPrefab).WithId("ExplosionPrefab").AsSingle();

        Container.BindInterfacesAndSelfTo<UnitMainController>().AsSingle();

        var c = transform.GetComponent<NavMeshMoveToMouse>();

        Container.QueueForInject(model);
    }

    protected abstract UnitModel CreateUnitModel();
}