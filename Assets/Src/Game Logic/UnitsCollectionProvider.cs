using System;
using System.Collections.Generic;
using Zenject;

public class UnitsCollectionProvider : IInitializable, IDisposable
{
    [Inject]
    private SignalBus _signalBus;

    public static readonly List<UnitModel> _units = new List<UnitModel>();

    public List<UnitModel> units => _units;
    public void Initialize()
    {
        _signalBus.Subscribe<UnitAddedSignal>(OnUnitAdded);
    }

    private void OnUnitAdded(UnitAddedSignal signal)
    {
        var unit = signal.unit;
        void OnUnitDestroyed()
        {
            _units.Remove(unit);
            unit.UnitDestroyed -= OnUnitDestroyed;
        };

        unit.UnitDestroyed += OnUnitDestroyed;
        _units.Add(unit);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<UnitAddedSignal>(OnUnitAdded);
    }
}
