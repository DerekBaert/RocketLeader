using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _body;
    [SerializeField] private GameObject _explosion;

    public Vector3 TargetPosition;
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Add force each frame in the direction it is facing
        _body.AddForce(transform.up);

        // When it reaches the position of the aiming reticle, explode.
        if(Vector3.Distance(TargetPosition, transform.position) < 0.5)
        {
            Instantiate(_explosion, TargetPosition, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
