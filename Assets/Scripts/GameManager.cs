using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _rocketsToSpawn = 10;
    [SerializeField] private float _rocketSpawnStartTimer = 1;
    private float _rocketSpawnTimer;
    public EnemyRocket _rocket;

    // Start is called before the first frame update
    void Start()
    {
        _rocketSpawnTimer = _rocketSpawnStartTimer;
    }

    // Update is called once per frame
    void Update()
    {
        _rocketSpawnTimer -= Time.deltaTime;
        if (_rocketSpawnTimer <= 0 && _rocketsToSpawn > 0)
        {
            Vector3 screen = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));

            _rocketSpawnTimer = _rocketSpawnStartTimer;

            float xPosition = Random.Range(0, screen.x);
            Instantiate(_rocket, new Vector3(xPosition, screen.y, 0), Quaternion.identity);
            _rocketsToSpawn--;
        }
        else
        {
            // Level complete
        }
    }
}
