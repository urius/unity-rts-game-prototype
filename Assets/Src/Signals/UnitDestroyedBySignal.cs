public class UnitDestroyedBySignal
{
    public readonly UnitFacade striker;
    public readonly UnitFacade hitUnit;

    public UnitDestroyedBySignal(UnitFacade hitUnit, UnitFacade striker)
    {
        this.hitUnit = hitUnit;
        this.striker = striker;
    }
}
