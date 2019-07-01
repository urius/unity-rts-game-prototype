using UnityEngine;
using Zenject;

public class DestroyableView : MonoBehaviour
{
    [Inject]
    private UnitModel _model;
    
    [SerializeField]
    private GameObject _explosionPrefab;
    [Inject]
    private IMoveAnimationAdapter _animationAdapter;
    

    [Inject]
    public virtual void Construct()
    {
        _model.UnitDestroyed += OnUnitDestroyed;
    }

    private void OnUnitDestroyed()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;

        var explosion = GameObject.Instantiate<GameObject>(_explosionPrefab, _model.position, _model.rotation);

        _animationAdapter.Dead();
    }

    protected virtual void Stop()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;
    }
}
