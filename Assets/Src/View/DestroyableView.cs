using UnityEngine;
using Zenject;

public class DestroyableView : MonoBehaviour
{
    [Inject]
    private UnitModel _model;
    
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private string _destroyAnimationName;
    

    public void OnEnable()
    {
        _model.UnitDestroyed += OnUnitDestroyed;
    }

    private void OnUnitDestroyed()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;

        var explosion = GameObject.Instantiate<GameObject>(_explosionPrefab, _model.position, _model.rotation);

        _animator.SetBool(_destroyAnimationName, true);
    }

    protected virtual void OnDisable()
    {
        _model.UnitDestroyed -= OnUnitDestroyed;
    }
}
