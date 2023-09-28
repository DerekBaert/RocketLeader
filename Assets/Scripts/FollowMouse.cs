using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Reticle object follows position of the mouse on the screen.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        mousePosition.z = 0;
        transform.position = mousePosition;
    }
}