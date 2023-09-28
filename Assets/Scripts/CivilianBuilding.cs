using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianBuilding : MonoBehaviour
{
    private bool _isAlive = true;
    [SerializeField] private GameObject _sprite;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = _sprite.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Returns the status of the building
    /// </summary>
    /// <returns>True if alive, false if not.</returns>
    public bool IsAlive()
    {
        return _isAlive;
    }

    /// <summary>
    /// Called when building is hit by an explosion or Rocket
    /// </summary>
    public void Hit()
    {
        _isAlive = false;
        _spriteRenderer.enabled = false;
    }
}
