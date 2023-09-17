using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRocket : MonoBehaviour
{
    private List<GameObject> _targets;
    private GameObject _target;
    private Rigidbody2D _body;

    private Vector3 _targetLocation;
    private Vector3 _vectorToTarget;

    public Transform RocketTransform;

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
            if (turret.isAlive)
            {
                _targets.Add(turret.gameObject);
            }
        }

        foreach (CivilianBuilding building in buildings)
        {
            if (building.isAlive)
            {
                _targets.Add(building.gameObject);
            }
        }
        // Debug.Log(_targets.Count);
        // Select random _target
        _target = _targets[Random.Range(0, _targets.Count)];

        // Rotate towards random _target in array
        _targetLocation = _target.transform.position;
        _vectorToTarget = _targetLocation - RocketTransform.position;
        float signedAngle = Vector2.SignedAngle(transform.up, _vectorToTarget);
        transform.rotation = Quaternion.Euler(0, 0, signedAngle);
    }

    // Update is called once per frame
    void Update()
    {
        // Move 'forward' relative to rotation
        _body.AddForce(new Vector2(_vectorToTarget.normalized.x, _vectorToTarget.normalized.y) * 0.5f);
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
