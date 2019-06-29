

using Zenject;

public class UnitFactoryFacade : UnitFacade
{
    [Inject]
    private UnitFactoryModel _model;

    public UnitFactoryModel UnitFactoryModel => _model;
}
