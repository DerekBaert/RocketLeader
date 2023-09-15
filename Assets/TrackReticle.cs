using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FollowTarget : MonoBehaviour
{
    private GameObject target;
    public GameObject projectile;
    public Transform turretTransform;
    [SerializeField] public GameObject cannonEnd;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Reticle");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetLocation = target.transform.position;
        Vector3 vectorToTarget = targetLocation - turretTransform.position;
        RotateTowardsTarget(targetLocation, vectorToTarget);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float signedAngle = Vector2.SignedAngle(transform.up, vectorToTarget);
            Instantiate(projectile, cannonEnd.transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(signedAngle * 0.5f, signedAngle * 1.5f)));
        }
    }

    private void RotateTowardsTarget(Vector3 targetLocation, Vector3 vectorToTarget)
    {
        float signedAngle = Vector2.SignedAngle(transform.up, vectorToTarget);
        turretTransform.rotation = Quaternion.Euler(0, 0, signedAngle);

        Debug.DrawRay(turretTransform.position, vectorToTarget, Color.red, .5f);
        Debug.DrawRay(turretTransform.position, transform.up, Color.green, .5f);
    }
}

