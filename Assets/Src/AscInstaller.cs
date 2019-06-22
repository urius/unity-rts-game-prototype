using UnityEngine;
using Zenject;

public class AscInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInstance<Animator>(gameObject.GetComponent<Animator>());

        Container.Bind<IMoveAnimationAdapter>()
            .To<ACSMoveAnimationAdapter>()
            .AsSingle();
    }
}