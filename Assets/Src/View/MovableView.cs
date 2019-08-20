using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public enum MoveStates
{
    Undefined,
    Idle,
    MovingForward,
    MovingBack,
    TurningRight,
    TurningLeft,
}


public class MovableView : MonoBehaviour
{
    [Serializable]
    private class AnimationSetting
    {
        public MoveStates state;
        public string animationName;
    }

    [Inject]
    private UnitModel _model;
    [Inject]
    private Animator _animator;


    [SerializeField]
    private float _stopAnimSpeed = 0.03f;
    [SerializeField]
    private float _stopAnimDistance = 2f;
    [SerializeField]
    private AnimationSetting[] _animationNames = new AnimationSetting[5];


    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private NavMeshAgent _navMeshAgent;
    private MoveStates _currentState = MoveStates.Undefined;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    private void OnEnable()
    {
        _model.UnitDestroyed += OnUnitDestroyed;
    }

    private void ChangeAnimationState(MoveStates moveState)
    {
        if (_currentState != moveState)
        {
            _currentState = moveState;
            var animationName = Array.Find(_animationNames, a => a.state == _currentState).animationName;
            _animator.SetBool(animationName, true);
        }
    }

    private void OnUnitDestroyed()
    {
        enabled = false;
    }

    private void OnDisable()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;
    }


    private Vector3[] _path => (_navMeshAgent.path != null && _navMeshAgent.path.corners.Length > 0) ?
                        _navMeshAgent.path.corners :
                        new Vector3[] { transform.position };

    void Update()
    {
        var deltaPos = Vector3.Distance(_lastPosition, transform.position);
        var deltaRot = Quaternion.Angle(_lastRotation, transform.rotation);
        var lastPathPoint = _path[_path.Length - 1];
        if (deltaPos > _stopAnimSpeed && Vector3.Distance(transform.position, lastPathPoint) > _stopAnimDistance)
        {
            ChangeAnimationState(MoveStates.MovingForward);
        }
        else if (deltaRot > 0)
        {
            var isRightDirection = Vector3.Cross(_lastRotation * transform.forward, transform.rotation * transform.forward).y > 0;
            ChangeAnimationState(isRightDirection ? MoveStates.TurningRight : MoveStates.TurningLeft);
        }
        else
        {
            ChangeAnimationState(MoveStates.Idle);
        }

        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }
}
