using UnityEngine;
using Zenject;

public class RobotsFactoryInstaller : UnitInstallerBase
{
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _cost;
    public override void InstallBindings()
    {
        base.InstallBindings();

        Container.BindInstance(gameObject.GetComponent<Animator>());

        Container.Bind<IMoveAnimationAdapter>()
            .To<RobotsFactoryAnimationAdapter>()
            .AsSingle();
    }

    protected override UnitModel CreateUnitModel()
    {
        return new UnitModel(team, _hp, _cost, 0);
    }
}