using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    public Rigidbody2D body;
    public Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.AddForce(transform.up);

        if(Vector3.Distance(targetPosition, transform.position) < 0.5)
        {
            Debug.Log("Explode");
            Destroy(gameObject);
        }
    }
}
