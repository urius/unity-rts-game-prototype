using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class UnitNavMeshMoveController : IInitializable, ITickable, IDisposable
{
    [Inject]
    private UnitModel _model;
    [Inject]
    private NavMeshAgent _agent;

    private Settings _settings;

    public UnitNavMeshMoveController(Settings settings)
    {
        _settings = settings;
    }

    public void Initialize()
    {
        _model.isActivePromise.Then(OnViewActivated);
    }

    private void OnViewActivated()
    {
        _model.DestinationChanged += OnDestinationChanged;
        _model.UnitDestroyed += OnUnitDestroyed;

        MoveToPosition(_model.destinationPoint);
    }

    private void OnUnitDestroyed()
    {
        _agent.enabled = false;
        Deactivate();
    }

    private void OnDestinationChanged(Vector3 newDestination)
    {
        MoveToPosition(newDestination);
    }

    private void Deactivate()
    {
        _model.DestinationChanged -= OnDestinationChanged;
        _model.UnitDestroyed -= OnUnitDestroyed;
    }

    public void MoveToPosition(Vector3 position)
    {
        if (_model.isAlive)
        {
            _agent.SetDestination(position);
        }
    }

    public void Tick()
    {
        if (!_model.isAlive)
        {
            return;
        }
        //var offset = new Vector3(0, 1, 0);
        //Debug.DrawLine(offset + gameObject.transform.position, offset + gameObject.transform.position + gameObject.transform.forward, Color.green);
        //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + _agent.desiredVelocity, Color.red);
        //Debug.DrawLine(_agent.pathEndPosition, _agent.pathEndPosition + transform.up * 10, Color.magenta);
        //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + transform.up * 10, Color.yellow);
        var firstPathPoint = _model.position;
        var lastPathPoint = _model.position;
        if (_agent.path != null && _agent.path.corners.Length > 0)
        {
            firstPathPoint = _agent.path.corners[0];
            lastPathPoint = _agent.path.corners[_agent.path.corners.Length - 1];
        }

        var angle = Vector3.Angle(_model.forward, _agent.desiredVelocity);
        if (angle > _settings.movingAngle)
        {
            _agent.velocity = Vector3.Lerp(_agent.velocity, Vector3.zero, 0.2f);
        }
    }
    public void Dispose()
    {
        Deactivate();
    }

    [Serializable]
    public class Settings
    {
        [SerializeField]
        private float _movingAngle = 30f;
        public float movingAngle => _movingAngle;
    }
}
