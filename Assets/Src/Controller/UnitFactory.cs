using UnityEngine;
using Zenject;

public class UnitFactory
{
    [Inject]
    private UnitsConfig _unitsConfig;
    [Inject]
    private DiContainer _container;

    public UnitFacade Create(MobileUnitType type, int team, Transform spawnTransform)
    {
        var subContainer = _container.CreateSubContainer();
        var settings = new MovableUnitInstaller.Parameters()
        {
            teamId = team
        };
        subContainer.BindInstance(settings);

        var unit = subContainer.InstantiatePrefab(_unitsConfig.GetConfigByType(type).prefab, spawnTransform.position, spawnTransform.rotation, null);

        return unit.GetComponent<UnitFacade>();
    }
}
