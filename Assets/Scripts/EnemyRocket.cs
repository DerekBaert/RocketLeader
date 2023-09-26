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
    [SerializeField] private GameObject _art;
    [SerializeField] private GameObject _trailRender;
    [SerializeField] private float _baseAcceleration = 0.01f;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float maxAcceleration;
    private List<GameObject> _targets;
    private GameObject _target;
    private Vector3 _targetLocation;
    private Vector3 _vectorToTarget;
    private float _currentAcceleration;
    private float _velocity;
    private Vector2 direction;
    private bool _isAlive = true;

    // Start is called before the first frame update
    void Start()
    {

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

        // Select random _target
        _target = _targets[Random.Range(0, _targets.Count)];

        // Determine the vector between the rocket and the target
        _targetLocation = _target.transform.position;
        _vectorToTarget = _targetLocation - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Determine direction to move based on vector to the target
        direction = _vectorToTarget.normalized * 0.001f;

        // Add to the current acceleration, clamp to max value
        _currentAcceleration += _baseAcceleration * Time.deltaTime;
        _currentAcceleration = Mathf.Clamp(_currentAcceleration, -maxAcceleration, maxAcceleration);

        // Add acceleration to velocity, clamp velocity
        _velocity += _currentAcceleration;
        _velocity = Mathf.Clamp(_velocity, 0, maxVelocity);

        // Multiply the direction to move by the velocity and apply to gameObject 
        Vector2 velocityVector = _velocity * direction;

        gameObject.transform.Translate(velocityVector);
    }

    public void Hit()
    {
        gameObject.SetActive(false);
        _isAlive = false;
    }

    public bool IsAlive()
    {
        return _isAlive;
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
