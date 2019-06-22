using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    public static readonly List<UnitAvatar> AllUnits = new List<UnitAvatar>();

    public int team = 0;

    [SerializeField]
    private int _hp = 100;

    [NonSerialized]
    public int cost;
    private int _maxHp;
    private TurretController _turretController;
    private StripeBar _hpBar;
    private UnityEngine.Object _explosionPrefab;

    [Inject]
    private IMoveAnimationAdapter _animationAdapter;
    void Awake()
    {
        AllUnits.Add(this);
        _maxHp = _hp;

        _turretController = GetComponent<TurretController>();
        _hpBar = GetComponentInChildren<StripeBar>();

        _explosionPrefab = Resources.Load("Explosion");
    }

    void Start()
    {
        UpdatePercent();
    }

    public int detectRadius => _turretController?.detectRadius ?? 0;
    public bool isAttacikng => _turretController?.isAttacking ?? false;
    public bool isLastShotHitTarget => _turretController?.isLastShotHitTarget ?? false;
    public int hp => _hp;
    public GameObject turretTarget => _turretController?.target?.gameObject;

    // Update is called once per frame
    public void DoDamage(int points)
    {
        if (_hp <= 0)
        {
            return;
        }
        _hp = Mathf.Max(0, _hp - points);
        UpdatePercent();

        if (_hp == 0)
        {
            AllUnits.Remove(this);
            var explosion = Instantiate(_explosionPrefab) as GameObject;
            explosion.transform.position = transform.position;

            var allBehaviours = GetComponents<Behaviour>();
            foreach (var behaviour in allBehaviours)
            {
                if (behaviour is Animator)
                {
                    continue;
                }
                behaviour.enabled = false;
            }

            _animationAdapter.Dead();

            StartCoroutine(WaitAndDestroy());
        }
    }

    private void UpdatePercent()
    {
        _hpBar?.SetPercent(100 * _hp / _maxHp);
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
