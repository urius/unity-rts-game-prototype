
using System;
using System.Collections.Generic;
using Zenject;

public class UnitsCollectionProvider : IInitializable, IDisposable
{
    [Inject]
    private SignalBus _signalBus;


    private readonly List<UnitModel> _units = new List<UnitModel>();

    public List<UnitModel> units => _units;

    public void Initialize()
    {
        _signalBus.Subscribe<UnitAddedSignal>(OnUnitAdded);
        _signalBus.Subscribe<UnitDestroyedSignal>(OnUnitDestroyed);
    }

    private void OnUnitAdded(UnitAddedSignal signal)
    {
        _units.Add(signal.unit);
    }

    private void OnUnitDestroyed(UnitDestroyedSignal signal)
    {
        _units.Remove(signal.unit);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<UnitAddedSignal>(OnUnitAdded);
        _signalBus.Unsubscribe<UnitDestroyedSignal>(OnUnitDestroyed);
    }
}
