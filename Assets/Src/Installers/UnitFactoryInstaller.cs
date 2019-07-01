using UnityEngine;

public class UnitFactoryInstaller : UnitInstallerBase
{
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _cost;
    [SerializeField]
    private UnitFactoryController.Settings _unitFactoryControllerSettings;
    protected override void InstallExtraBindings()
    {
        Container.BindInstance(gameObject.GetComponent<Animator>());

        Container.BindInstance<UnitFactoryModel>(new UnitFactoryModel()).AsSingle();

        Container.BindInterfacesAndSelfTo<UnitFactoryController>().AsSingle().WithArguments(_unitFactoryControllerSettings);
    }

    protected override UnitModel CreateUnitModel()
    {
        return new UnitModel(team, _hp, _cost, 0);
    }
}