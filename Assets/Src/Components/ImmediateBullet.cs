using UnityEngine;

public class ImmediateBullet : BulletBase
{
    public float fadeOutSpeed = 0.02f;
    private float alpha = 0.6f;
    private LineRenderer _lineRenderer;

    private UnityEngine.Object _sparksPrefab;

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
                if (unit.UnitModel == target)
                {
                    hitPromise.Resolve(unit);
                }
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
            hitPromise.Resolve(null);

            Destroy(gameObject);
        }
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
