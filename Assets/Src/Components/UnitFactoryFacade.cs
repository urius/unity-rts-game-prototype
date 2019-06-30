using Zenject;

public class UnitFactoryFacade : UnitFacade
{
    [Inject]
    private UnitsConfig _unitsConfig;
    [Inject]
    private GameData _gameData;

    [Inject]
    private UnitFactoryModel _factoryModel;
    public IReadonlyFactoryModel FactoryModel => _factoryModel;

    public bool TryBuildUnit(MobileUnitType type)
    {
        var buildingUnitConfig = _unitsConfig.GetConfigByType(type);
        if (_gameData.TryChangePlayerMoney(UnitModel.teamId, -buildingUnitConfig.cost))
        {
            _factoryModel.AddToBuildQueue(type);
            return true;
        }

        return false;
    }
}
