using UnityEngine;
using Zenject;

public class UnitFactoryInstaller : UnitInstallerBase
{
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _cost;
    [SerializeField]
    private UnitFactoryController.Settings _unitFactoryControllerSettings;
    public override void InstallBindings()
    {
        base.InstallBindings();

        Container.BindInstance(gameObject.GetComponent<Animator>());

        Container.BindInstance<UnitFactoryModel>(new UnitFactoryModel()).AsSingle();

        Container.BindInterfacesAndSelfTo<UnitFactoryController>().AsSingle().WithArguments(_unitFactoryControllerSettings);

        Container.Bind<IMoveAnimationAdapter>()
            .To<RobotsFactoryAnimationAdapter>()
            .AsSingle();
    }

    protected override UnitModel CreateUnitModel()
    {
        return new UnitModel(team, _hp, _cost, 0);
    }
}