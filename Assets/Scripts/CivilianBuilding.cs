using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianBuilding : MonoBehaviour
{
    private bool isAlive = true;
    public GameObject Sprite;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = Sprite.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAlive()
    {
        return isAlive;
    }


    public void Hit()
    {
        isAlive = false;
        spriteRenderer.enabled = false;
    }
}
