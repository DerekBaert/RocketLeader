using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyRocket : MonoBehaviour
{
    private List<GameObject> _targets;
    public GameObject _target;
    private Rigidbody2D _body;
    private Vector3 _targetLocation;
    private Vector3 _vectorToTarget;
    public Transform rocketTransform;
    private float _currentAcceleration;
    private float _velocity;
    private Vector2 direction;

    [SerializeField] private GameObject _art;
    [SerializeField] private float _baseAcceleration = 0.01f;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float maxAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _targets = new List<GameObject>();

        // Get all possible _targets (turrets and buildings)
        Turret[] turrets = FindObjectsByType<Turret>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        CivilianBuilding[] buildings = FindObjectsByType<CivilianBuilding>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        
        // Add all possible _targets to one array
        foreach (Turret turret in turrets)
        {
            if (turret.IsAlive())
            {
                _targets.Add(turret.gameObject);
            }
        }

        foreach (CivilianBuilding building in buildings)
        {
            if (building.IsAlive())
            {
                _targets.Add(building.gameObject);
            }
        }
        // Debug.Log(_targets.Count);
        // Select random _target
        _target = _targets[Random.Range(0, _targets.Count)];

        // Rotate towards random _target in array
        _targetLocation = _target.transform.position;
        _vectorToTarget = _targetLocation - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move 'forward' relative to rotation
        direction = _vectorToTarget.normalized * 0.001f;

        _currentAcceleration += _baseAcceleration * Time.deltaTime;
        _currentAcceleration = Mathf.Clamp(_currentAcceleration, -maxAcceleration, maxAcceleration);

        _velocity += _currentAcceleration;
        _velocity = Mathf.Clamp(_velocity, 0, maxVelocity);

        Vector2 velocityVector = _velocity * direction;

        gameObject.transform.Translate(velocityVector);
    }

    public void Hit()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject target = other.gameObject;
        switch (target.tag)
        {
            case "PlayerBuilding":
                target.GetComponent<CivilianBuilding>().Hit();
                Destroy(gameObject);
                break;
            case "PlayerTurret":
                target.GetComponent<Turret>().Hit();
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
