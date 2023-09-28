using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using static UnityEngine.GraphicsBuffer;
using Application = UnityEngine.Application;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private GameObject _reticle;
    [SerializeField] private int _rocketsToSpawn = 10;
    [SerializeField] private float _rocketSpawnRate = 2;
    [SerializeField] private EndGameScreen _endGameScreen;
    private float _rocketSpawnCountdown;
    private float _rocketsSpawnedCount;
    private bool _scoreDisplayed = false;
    private Turret[] _turrets;
    private CivilianBuilding[] _buildings;
    private float _rocketsDestroyed;
    private List<EnemyRocket> _rocketsSpawned = new List<EnemyRocket>();
    public EnemyRocket Rocket;

    

    // Start is called before the first frame update
    void Start()
    {
        _rocketSpawnCountdown = _rocketSpawnRate;
        _turrets = FindObjectsByType<Turret>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        _buildings = FindObjectsByType<CivilianBuilding>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        UnityEngine.Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrement countdown, when it reaches 0.
        _rocketSpawnCountdown -= Time.deltaTime;
        
        // When timer reaches 0, if there are still rockets to spawn, spawn a rocket.
        if (_rocketSpawnCountdown <= 0 && _rocketsSpawnedCount < _rocketsToSpawn)
        {
            SpawnRockets();
        }
        else if(!_scoreDisplayed && _rocketsDestroyed >= _rocketsToSpawn)
        {
            // If the score hasn't been displayed and all rockets have been spawned,
            // then calculate player score, pass to end screen, and display end screen.
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
            _endGameScreen.Setup(ammoScore, buildingScore, finalScore);
            _scoreDisplayed = true;
        }

        // When the spacebar is pressed, fire a rocket.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireRocket();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // If there are rockets currently spawned, check if any have been destroyed,
        // remove them and increment counter.
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

    }

    /// <summary>
    /// // Spawns a rocket at a random point at the top of the screen.
    ///     Rocket is then added to a list to track rockets spawned
    /// </summary>
    private void SpawnRockets()
    {
        Vector3 screen =
            Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));

        _rocketSpawnCountdown = _rocketSpawnRate;

        float xPosition = Random.Range(-((screen.x / 2) + 2), (screen.x - 2));
        EnemyRocket rocket = Instantiate(this.Rocket, new Vector3(xPosition, screen.y - 1.5f, 0), Quaternion.identity);
        _rocketsSpawned.Add(rocket);

        _rocketsSpawnedCount++;
    }

    /// <summary>
    /// Locates the turret closest to the aiming reticle and fires a rocket from it.
    /// </summary>
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
