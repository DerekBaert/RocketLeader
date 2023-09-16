using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float timeToLive = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive < 0 )
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject target = other.gameObject;
        switch (target.tag)
        {
            case "PlayerBuilding":
                target.GetComponent<CivilianBuilding>().Hit();
                break;
            case "PlayerTurret":
                target.GetComponent<Turret>().Hit();
                break;
            case "EnemyRocket":
                target.GetComponent<EnemyRocket>().Hit();
                break;
            default:
                break;
        }
    }
}
