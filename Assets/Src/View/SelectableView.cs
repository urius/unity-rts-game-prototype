using System;
using UnityEngine;
using Zenject;

public class SelectableView : MonoBehaviour
{
    public event Action MouseDown = delegate { };
    public event Action MouseUp = delegate { };

    [Inject]
    private UnitModel _model;


    [SerializeField]
    private bool _isSelected = false;
    [SerializeField]
    private GameObject _selection;


    [SerializeField]
    private Collider _selectionArea;

    private void OnEnable()
    {
        _selection.SetActive(_isSelected);

        _model.SelectionChanged += OnSelectionChanged;
        _model.UnitDestroyed += OnUnitDestroyed;
    }

    void OnMouseDown()
    {
        if (isActiveAndEnabled)
        {
            var caneraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_selectionArea.Raycast(caneraRay, out var _, int.MaxValue))
            {
                MouseDown();
            }
        }
    }

    void OnMouseUp()
    {
        MouseUp();
    }

    private void OnUnitDestroyed()
    {
        enabled = false;
    }

    private void OnSelectionChanged(bool isSelected)
    {
        _isSelected = isSelected;
        _selection.SetActive(isSelected);
    }

    private void OnDisable()
    {
        _selection.SetActive(false);
        
        _model.SelectionChanged -= OnSelectionChanged;
        _model.UnitDestroyed -= OnUnitDestroyed;
    }

    // Editor only!
    void OnValidate()
    {
        _selection.SetActive(_isSelected);
    }
}
