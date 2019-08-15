using System;
using UnityEngine;
using Zenject;

public abstract class UnitInstallerBase : MonoInstaller
{
    [SerializeField]
    protected Parameters parameters;

    private UnitModel _model;

    public override void Start()
    {
        base.Start();
        
        _model.Activate();
    }

    public override void InstallBindings()
    {
        var injectedParameters = Container.TryResolve<Parameters>();
        if (injectedParameters != null)
        {
            parameters = injectedParameters;
        }

        Container.BindInstance<UnitFacade>(transform.GetComponent<UnitFacade>()).AsSingle();

        _model = CreateUnitModel();
        Container.BindInstance<UnitModel>(_model).AsSingle();

        Container.BindInstance<Transform>(transform).AsSingle();
        Container.BindInstance<GameObject>(transform.gameObject).AsSingle();

        Container.BindInterfacesAndSelfTo<UnitMainController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnitDisplayHpController>().AsSingle().NonLazy();

        var hpBar = transform.GetComponentInChildren<StripeBar>();
        Container.BindInstance(hpBar).WhenInjectedInto<UnitDisplayHpController>();


        InstallExtraBindings();

        Container.QueueForInject(_model);
    }

    protected abstract UnitModel CreateUnitModel();
    protected abstract void InstallExtraBindings();

    [Serializable]
    public class Parameters
    {
        public int teamId = -1;
    }
}