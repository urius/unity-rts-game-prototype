using UnityEngine;
using Zenject;

public class UnitFactoryCanvasInstaller : MonoInstaller
{
    [SerializeField]
    private UnitFactoryFacade _unitFactoryFacade;
    
    override public void InstallBindings() {
        Container.BindInstance<UnitFactoryFacade>(_unitFactoryFacade).AsSingle();
    }
}
