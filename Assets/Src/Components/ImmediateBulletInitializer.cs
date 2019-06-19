using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ImmediateBulletInitializer : BulletInitializerBase
{
    // Start is called before the first frame update
    public float fadeOutSpeed = 0.02f;
    private float alpha = 0.6f;
    private LineRenderer _lineRenderer;

    private UnityEngine.Object _sparksPrefab;

    private UnitAvatar target;
    private Vector3 from;
    private Vector3 direction;
    private int damage;
    private TaskCompletionSource<UnitAvatar> _hitTaskCompletionSource;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _sparksPrefab = Resources.Load("Sparks");
    }
    void Start()
    {
        StartBullet();
    }

    private void StartBullet()
    {
        if (Physics.Raycast(from, direction, out var hit))
        {
            var unit = hit.transform.gameObject.GetComponent<UnitAvatar>();
            var hitPoint = hit.point;
            if (unit != null)
            {
                hitPoint = unit.gameObject.GetComponent<Collider>().bounds.center;
                unit.DoDamage(damage);
                if (unit == target)
                {
                    _hitTaskCompletionSource.TrySetResult(unit);
                }
            }

            if (_hitTaskCompletionSource.Task.IsCompleted == false)
            {
                _hitTaskCompletionSource.TrySetResult(null);
            }

            _lineRenderer.SetPositions(new Vector3[] {
                        from,
                        hitPoint
                    });

            var sparks = Instantiate(_sparksPrefab) as GameObject;
            sparks.transform.position = hitPoint - direction.normalized;
            sparks.transform.rotation = Quaternion.LookRotation(-direction, transform.up);
        }
        else
        {
            _hitTaskCompletionSource.TrySetResult(null);

            Destroy(gameObject);
        }
    }

    protected override Task<UnitAvatar> InitializeInternal(UnitAvatar striker, UnitAvatar target, Vector3 from, Vector3 direction, int damage)
    {
        this.target = target;
        this.from = from;
        this.direction = direction;
        this.damage = damage;

        _hitTaskCompletionSource = new TaskCompletionSource<UnitAvatar>();
        return _hitTaskCompletionSource.Task;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Random.Range(0, 100) < 2)
        {
            var c = _lineRenderer.material.color;
            alpha -= fadeOutSpeed;
            if (alpha <= 0.05f)
            {
                Destroy(gameObject);
                return;
            }

            _lineRenderer.material.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
