using RSG;

public interface IBullet
{
    IPromise<UnitModel> HitPromise { get; }
}
