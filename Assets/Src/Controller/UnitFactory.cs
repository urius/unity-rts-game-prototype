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
        var unit = GameObject.Instantiate(_unitsConfig.GetConfigByType(type).prefab, spawnTransform.position, spawnTransform.rotation) as GameObject;
        unit.GetComponent<UnitInstallerBase>().SetupTeam(team);
        _container.InjectGameObject(unit);

        return unit.GetComponent<UnitFacade>();
    }
}
