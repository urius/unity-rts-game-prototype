using System;
using System.Collections.Generic;
using Zenject;

public class UnitsCollectionProvider : IInitializable, IDisposable
{
    [Inject]
    private SignalBus _signalBus;

    public static readonly List<UnitFacade> _units = new List<UnitFacade>();

    public List<UnitFacade> units => _units;
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
            unit.UnitModel.UnitDestroyed -= OnUnitDestroyed;
        };

        unit.UnitModel.UnitDestroyed += OnUnitDestroyed;
        _units.Add(unit);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<UnitAddedSignal>(OnUnitAdded);
    }
}
