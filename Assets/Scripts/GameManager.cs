using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _rocketsToSpawn = 10;
    [SerializeField] private float _rocketSpawnRate = 2;
    private float _rocketSpawnCounter;
    private float _rocketsSpawned;
    public EnemyRocket _rocket;
    private Turret[] _turrets;
    [SerializeField] private GameObject _reticle;
    private Vector3 _reticleLocation;

    // Start is called before the first frame update
    void Start()
    {
        _rocketSpawnCounter = _rocketSpawnRate;
        _turrets = FindObjectsByType<Turret>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        _rocketSpawnCounter -= Time.deltaTime;
        _reticleLocation = _reticle.transform.position;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            FireRocket();
        }

        if (_rocketSpawnCounter <= 0 && _rocketsSpawned < _rocketsToSpawn)
        {
            SpawnRockets();
        }
        else
        {
            // Level complete
        }
    }

    private void SpawnRockets()
    {
        Vector3 screen =
            Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));

        _rocketSpawnCounter = _rocketSpawnRate;

        float xPosition = Random.Range(2, (screen.x - 2));
        Instantiate(_rocket, new Vector3(xPosition, screen.y, 0), Quaternion.identity);
        _rocketsSpawned++;

        if (_rocketsSpawned % 3 == 0)
        {
            _rocketSpawnRate -= 0.5f;
        }
    }

    private void FireRocket()
    {
        Turret nearestTurret = _turrets[0];
        float nearestTurretDistance = Vector3.Distance(_reticle.transform.position, nearestTurret.transform.position);

        foreach (var turret in _turrets)
        {
            if (turret.CanFireRocket())
            {
                float currentTurretDistance =
                    Vector3.Distance(_reticle.transform.position, turret.transform.position);
                if (currentTurretDistance < nearestTurretDistance)
                {
                    nearestTurret = turret;
                    nearestTurretDistance = currentTurretDistance;
                }
            }
        }

        if (nearestTurret.CanFireRocket())
        {
            nearestTurret.FireRocket();
        }
    }
}
