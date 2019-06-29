using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class UnitInstallerBase : MonoInstaller
{
    [SerializeField]
    protected int team;

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
        Container.BindInstance<GameObject>(transform.gameObject).AsSingle();

        Container.BindInterfacesAndSelfTo<UnitMainController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnitDisplayHpController>().AsSingle().NonLazy();

        var hpBar = transform.GetComponentInChildren<StripeBar>();
        Container.BindInstance(hpBar).WhenInjectedInto<UnitDisplayHpController>();

        Container.QueueForInject(model);
    }

    protected abstract UnitModel CreateUnitModel();
}