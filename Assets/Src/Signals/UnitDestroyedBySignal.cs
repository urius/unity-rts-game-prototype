public class UnitDestroyedBySignal
{
    public readonly UnitModel striker;
    public readonly UnitModel target;

    public UnitDestroyedBySignal(UnitModel striker, UnitModel target)
    {
        this.striker = striker;
        this.target = target;
    }
}
