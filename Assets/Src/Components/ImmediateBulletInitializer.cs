using System.Threading.Tasks;
using UnityEngine;

public class ImmediateBulletInitializer : BulletInitializerBase
{
    public float fadeOutSpeed = 0.02f;
    private float alpha = 0.6f;
    private LineRenderer _lineRenderer;

    private UnityEngine.Object _sparksPrefab;

    private UnitModel target;
    private Vector3 from;
    private Vector3 direction;
    private int damage;
    private TaskCompletionSource<UnitModel> _hitTaskCompletionSource;

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
            var unit = hit.transform.gameObject.GetComponent<UnitFacade>();
            var hitPoint = hit.point;
            if (unit != null)
            {
                hitPoint = unit.gameObject.GetComponent<Collider>().bounds.center;
                unit.UnitModel.DoDamage(damage);
                if (unit.UnitModel == target)
                {
                    _hitTaskCompletionSource.TrySetResult(unit.UnitModel);
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

    protected override Task<UnitModel> InitializeInternal(UnitModel striker, UnitModel target, Vector3 from, Vector3 direction, int damage)
    {
        this.target = target;
        this.from = from;
        this.direction = direction;
        this.damage = damage;

        _hitTaskCompletionSource = new TaskCompletionSource<UnitModel>();
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
