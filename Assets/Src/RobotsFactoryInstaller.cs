using UnityEngine;
using Zenject;

public class RobotsFactoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInstance(gameObject.GetComponent<Animator>());

        Container.Bind<IMoveAnimationAdapter>()
            .To<RobotsFactoryAnimationAdapter>()
            .AsSingle();
    }
}