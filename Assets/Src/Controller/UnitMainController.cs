using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class UnitMainController : IInitializable, IDisposable
{
    [Inject]
    private SignalBus _signalBus;
    [Inject]
    private GameData _gameData;

    [Inject]
    private UnitModel _model;
    [Inject]
    private IMoveAnimationAdapter _animationAdapter;
    [Inject(Id = "ExplosionPrefab")]
    private GameObject _explosionPrefab;
    [Inject]
    private CoroutinesManager _coroutinesManager;
    [Inject]
    private Transform _transform;

    public void Initialize()
    {
        _model.HealthUpdated += OnHealthUpdated;

        _signalBus.Fire(new UnitAddedSignal { unit = _model });


        //TODO: Temporary hack
        if (_model.teamId == _gameData.UserTeam)
        {
            var moveToMouse = _transform.GetComponent<NavMeshMoveToMouse>();
            if (moveToMouse != null)
            {
                moveToMouse.enabled = true;
            }
            var selectable = _transform.GetComponent<Selectable>();
            if (selectable != null)
            {
                selectable.enabled = true;
            }
        }
    }

    private void OnHealthUpdated(int value)
    {
        if (value <= 0)
        {
            _model.HealthUpdated -= OnHealthUpdated;

            _signalBus.Fire(new UnitDestroyedSignal { unit = _model });

            var explosion = GameObject.Instantiate<GameObject>(_explosionPrefab, _model.transform);

            //TODO temporary hack
            // var allBehaviours = _transform.GetComponents<Behaviour>();
            // foreach (var behaviour in allBehaviours)
            // {
            //     if (behaviour is Animator)
            //     {
            //         continue;
            //     }
            //     behaviour.enabled = false;
            // }

            _animationAdapter.Dead();

            _coroutinesManager.StartCoroutine(WaitAndDestroy());
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(10);
        GameObject.Destroy(_transform.gameObject);
    }

    public void Dispose()
    {
        _model.HealthUpdated -= OnHealthUpdated;
    }
}
