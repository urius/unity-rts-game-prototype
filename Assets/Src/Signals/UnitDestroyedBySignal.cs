public class UnitDestroyedBySignal
{
    public readonly UnitModel striker;
    public readonly UnitModel hitUnit;

    public UnitDestroyedBySignal(UnitModel hitUnit, UnitModel striker)
    {
        this.hitUnit = hitUnit;
        this.striker = striker;
    }
}
