using Zenject;

public class UnitFacade : AwakableView
{
    [Inject]
    protected UnitModel model;

    public UnitModel UnitModel => model;
}
