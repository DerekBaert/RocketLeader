using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

public class GameManager : MonoBehaviour
{
    public int rocketsToSpawn = 10;
    public float rocketSpawnStartTimer = 5;
    private float _rocketSpawnTimer;
    public EnemyRocket rocket;

    // Start is called before the first frame update
    void Start()
    {
        _rocketSpawnTimer = rocketSpawnStartTimer;
    }

    // Update is called once per frame
    void Update()
    {
        _rocketSpawnTimer -= Time.deltaTime;
        if (_rocketSpawnTimer <= 0)
        {
            int rocketsToSpawn = Random.Range(2, 5);
            //Debug.Log("Spawning " + rocketsToSpawn +" rockets.");
            Vector3 screen = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));
            
            _rocketSpawnTimer = rocketSpawnStartTimer;
            for (int i = 0; i < rocketsToSpawn; i++)
            {
                float xPosition = Random.Range(-(screen.x / 2), screen.x / 2);
                Instantiate(rocket, new Vector3(xPosition, screen.y, 0), Quaternion.identity);
            }
            
        }
    }
}
