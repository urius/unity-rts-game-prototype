using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private GameData _gameData;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<MoneyChangedSignal>();
        Container.DeclareSignal<UnitDestroyedBySignal>();
        Container.DeclareSignal<UnitAddedSignal>();

        Container.QueueForInject(_gameData);
        Container.BindInterfacesAndSelfTo<GameData>().FromInstance(_gameData).AsSingle();
        Container.Bind<UnitFactory>().AsSingle();
        Container.Bind<BulletFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<UnitsCollectionProvider>().AsSingle();

        Container.BindInstance(GetComponent<CoroutinesHolder>()).WhenInjectedInto<CoroutinesManager>();
        Container.BindInterfacesAndSelfTo<CoroutinesManager>().AsTransient();

    }
}