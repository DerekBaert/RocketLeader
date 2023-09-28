using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float _timeToLive = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Decrement time to live, when this reaches 0 the object is destroyed.
        _timeToLive -= Time.deltaTime;

        if (_timeToLive < 0 )
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called when overlapping another collider.
    /// </summary>
    /// <param name="other">Other object that triggered the collision.</param>
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
