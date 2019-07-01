using System;
using UnityEngine;
using Zenject;

public class UnitMoveByUserController : IInitializable, IDisposable
{
    [Inject]
    private UnitModel _model;
    [Inject]
    private SelectableDestroyableView _view;

    public void Initialize()
    {
        _view.MouseDown += OnMouseDown;
        _view.MouseUp += OnMouseUp;
    }

    public void Dispose()
    {
        _view.MouseDown -= OnMouseDown;
        _view.MouseUp -= OnMouseUp;
    }

    private void OnMouseDown()
    {
        _model.isSelected = true;
    }
    private void OnMouseUp()
    {
        if (_model.isSelected)
        {
            _model.isSelected = false;
            if (_model.isAlive)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    var targetPosition = hit.point;
                    targetPosition.y = _model.position.y;

                    _model.destinationPoint = targetPosition;
                }
            }
        }
    }
}
