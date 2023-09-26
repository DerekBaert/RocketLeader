using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private GameObject _reticle;
    [SerializeField] private int _rocketsToSpawn = 10;
    [SerializeField] private float _rocketSpawnRate = 2;
    [SerializeField] private EndGameScreen _endGameScreen;

    private float _rocketSpawnCountdown;
    private float _rocketsSpawnedCount;
    public EnemyRocket _rocket;
    private Turret[] _turrets;
    private CivilianBuilding[] _buildings;
    private float _rocketsDestroyed;
    private List<EnemyRocket> _rocketsSpawned = new List<EnemyRocket>();
    

    private bool _scoreDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        _rocketSpawnCountdown = _rocketSpawnRate;
        _turrets = FindObjectsByType<Turret>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        _buildings = FindObjectsByType<CivilianBuilding>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        _rocketSpawnCountdown -= Time.deltaTime;

        if (_rocketsSpawned.Count > 0)
        {
            foreach (var enemyRocket in _rocketsSpawned)
            {
                if (!enemyRocket.IsAlive() && enemyRocket != null)
                {
                    _rocketsDestroyed++;
                    Destroy(enemyRocket);
                }
            }
        }
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            FireRocket();
        }

        if (_rocketSpawnCountdown <= 0 && _rocketsSpawnedCount < _rocketsToSpawn)
        {
            SpawnRockets();
        }
        else if(!_scoreDisplayed && _rocketsDestroyed >= _rocketsToSpawn)
        {
            int buildingScore = 0;
            int ammoScore = 0;
            int finalScore = 0;
            foreach (var building in _buildings)
            {
                if (building.IsAlive())
                {
                    buildingScore += 20;
                }
            }

            foreach (var turret in _turrets)
            {
                if (turret.IsAlive())
                {
                    ammoScore += turret.GetAmmoRemaining() * 5;
                }
            }

            finalScore += ammoScore + buildingScore;
            //Debug.Log("Buildings remaining: " + buildingScore);
            //Debug.Log("Ammo remaining: " + ammoScore);
            //Debug.Log("Total Score: " + finalScore);
            _endGameScreen.Setup(ammoScore, buildingScore, finalScore);
            _scoreDisplayed = true;
        }
            
    }

    private void SpawnRockets()
    {
        Vector3 screen =
            Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));

        _rocketSpawnCountdown = _rocketSpawnRate;

        float xPosition = Random.Range(-((screen.x / 2) + 2), (screen.x - 2));
        EnemyRocket rocket = Instantiate(_rocket, new Vector3(xPosition, screen.y - 1.5f, 0), Quaternion.identity);
        _rocketsSpawned.Add(rocket);

        _rocketsSpawnedCount++;
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
