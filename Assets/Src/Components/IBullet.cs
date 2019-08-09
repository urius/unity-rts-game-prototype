using RSG;

public interface IBullet
{
    IPromise<UnitFacade> HitPromise { get; }
}
