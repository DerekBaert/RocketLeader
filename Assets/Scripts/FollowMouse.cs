using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        mousePosition.z = 0;
        //Debug.Log(mousePosition);
        transform.position = mousePosition;
    }
}