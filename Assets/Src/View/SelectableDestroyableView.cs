using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SelectableDestroyableView : DestroyableView
{
    public event Action MouseDown = delegate { };
    public event Action MouseUp = delegate { };

    [Inject]
    private UnitModel _model;
    [Inject]
    private IMoveAnimationAdapter _animationAdapter;

    [SerializeField]
    private bool _isSelected = false;
    [SerializeField]
    private GameObject _selection;

    [SerializeField]
    private Collider _selectionArea;
    [SerializeField]
    private float _stopAnimSpeed = 0.03f;
    [SerializeField]
    private float _stopAnimDistance = 2f;


    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private NavMeshAgent _navMeshAgent;

    [Inject]
    public override void Construct()
    {
        base.Construct();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _selection.SetActive(_isSelected);

        _lastPosition = _model.transform.position;
        _lastRotation = _model.transform.rotation;

        _model.SelectionChanged += OnSelectionChanged;
        _model.UnitDestroyed += OnUnitDestroyed;
    }

    private void OnUnitDestroyed()
    {
        enabled = false;
    }

    override protected void Stop()
    {
        base.Stop();

        _selection.SetActive(false);
        _model.SelectionChanged -= OnSelectionChanged;
        _model.UnitDestroyed -= OnUnitDestroyed;
    }


    private Vector3[] path => (_navMeshAgent.path != null && _navMeshAgent.path.corners.Length > 0) ?
                        _navMeshAgent.path.corners :
                        new Vector3[] { transform.position };

    void Update()
    {
        var transform = _model.transform;
        var deltaPos = Vector3.Distance(_lastPosition, transform.position);
        var deltaRot = Quaternion.Angle(_lastRotation, transform.rotation);
        var lastPathPoint = path[path.Length - 1];
        if (deltaPos > _stopAnimSpeed && Vector3.Distance(transform.position, lastPathPoint) > _stopAnimDistance)
        {
            _animationAdapter.ChangeMoveState(MoveStates.MovingForward);
        }
        else if (deltaRot > 0)
        {
            var isRightDirection = Vector3.Cross(_lastRotation * transform.forward, transform.rotation * transform.forward).y > 0;
            _animationAdapter.ChangeMoveState(isRightDirection ? MoveStates.TurningRight : MoveStates.TurningLeft);
        }
        else
        {
            _animationAdapter.ChangeMoveState(MoveStates.Idle);
        }

        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
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

    // Editor only!
    void OnValidate()
    {
        _selection.SetActive(_isSelected);
    }

    private void OnSelectionChanged(bool isSelected)
    {
        _isSelected = isSelected;
        _selection.SetActive(isSelected);
    }
}
