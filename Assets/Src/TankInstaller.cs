using UnityEngine;
using Zenject;

public class TankInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("TankInstaller call");

        Container.BindInstance(gameObject.GetComponent<Animator>());
        
        Container.Bind<IMoveAnimationAdapter>()
            .To<TankCannonMoveAnimationAdapter>()
            .AsSingle();
    }
}