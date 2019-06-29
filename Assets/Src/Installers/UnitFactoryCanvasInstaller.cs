using UnityEngine;
using Zenject;

public class UnitFactoryCanvasInstaller : MonoInstaller
{
    [SerializeField]
    private UnitFactoryFacade _unitFactoryFacade;
    // Start is called before the first frame update
    override public void InstallBindings() {
        Container.BindInstance<UnitFactoryModel>(_unitFactoryFacade.UnitFactoryModel).AsSingle();
    }
}
