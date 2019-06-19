using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMoveDecorator : MonoBehaviour
{
    [SerializeField]
    private float _movingAngle = 30f;
    [SerializeField]
    private float _stopAnimSpeed = 0.03f;
    [SerializeField]
    private float _stopAnimDistance= 2f;
    private NavMeshAgent _agent;
    private IMoveAnimationAdapter _moveAnimationAdapter;
    private Quaternion _lastRotation;
    private Vector3 _lastPosition;
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _moveAnimationAdapter = GetComponent<IMoveAnimationAdapter>();
    }

    void Start()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    public void MoveToPosition(Vector3 position)
    {
        if (_agent.isActiveAndEnabled)
        {
            _agent.SetDestination(position);
        }
    }
    
    void Update()
    {
        //var offset = new Vector3(0, 1, 0);
        //Debug.DrawLine(offset + gameObject.transform.position, offset + gameObject.transform.position + gameObject.transform.forward, Color.green);
        //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + _agent.desiredVelocity, Color.red);
        //Debug.DrawLine(_agent.pathEndPosition, _agent.pathEndPosition + transform.up * 10, Color.magenta);
        //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + transform.up * 10, Color.yellow);
        var firstPathPoint = transform.position;
        var lastPathPoint = transform.position;
        if (_agent.path != null && _agent.path.corners.Length > 0)
        {
            firstPathPoint = _agent.path.corners[0];
            lastPathPoint = _agent.path.corners[_agent.path.corners.Length - 1];
        }

        var angle = Vector3.Angle(gameObject.transform.forward, _agent.desiredVelocity);
        if (angle > _movingAngle)
        {
            _agent.velocity = Vector3.Lerp(_agent.velocity, Vector3.zero, 0.2f);
        }

        var deltaPos = Vector3.Distance(_lastPosition, transform.position);
        var deltaRot = Quaternion.Angle(_lastRotation, transform.rotation);
        if (deltaPos > _stopAnimSpeed && Vector3.Distance(transform.position, lastPathPoint) > _stopAnimDistance)
        {
            _moveAnimationAdapter.ChangeMoveState(MoveStates.MovingForward);
        }
        else if (deltaRot > 0)
        {
            var isRightDirection = Vector3.Cross(gameObject.transform.forward, _agent.desiredVelocity).y > 0;
            _moveAnimationAdapter.ChangeMoveState(isRightDirection ? MoveStates.TurningRight : MoveStates.TurningLeft);
        }
        else
        {
            _moveAnimationAdapter.ChangeMoveState(MoveStates.Idle);
        }

        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }
}
