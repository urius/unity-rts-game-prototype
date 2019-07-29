using UnityEngine;

public class StraightFlyBullet : BulletBase
{
    [SerializeField]
    private float _speed = 10;

    private const int _maxDistance = 1000;
    private UnityEngine.Object _sparksPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _sparksPrefab = Resources.Load("Sparks");
        transform.position = from;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    // Update is called once per frame
    void Update()
    {
        var oldPosition = transform.position;
        transform.Translate(0, 0, Time.deltaTime * _speed);

        if (Vector3.Distance(from, transform.position) > _maxDistance)
        {
            Destroy(gameObject);
        }
        else if (Physics.Linecast(oldPosition, transform.position, out var hit))
        {
            var unit = hit.transform.gameObject.GetComponent<UnitFacade>();
            var hitPoint = hit.point;
            if (unit != null)
            {
                hitPoint = unit.gameObject.GetComponent<Collider>().bounds.center;
                if (unit.UnitModel == target)
                {
                    hitPromise.Resolve(unit.UnitModel);
                }
            }

            var sparks = Instantiate(_sparksPrefab) as GameObject;
            var direction = (transform.position - oldPosition).normalized;
            sparks.transform.position = hitPoint - direction;
            sparks.transform.rotation = Quaternion.LookRotation(-direction, transform.up);

            Destroy(gameObject);
        };
    }
}
