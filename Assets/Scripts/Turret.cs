using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject _sprite;
    [SerializeField] private GameObject _reticle;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private GameObject _cannonEnd;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private int _ammo = 10;
    [SerializeField] private TMP_Text _text;

    private bool _isAlive = true;
    private float _lerpSpeed = 0.25f;
    private float _rotateTimer;
    private float _rotateTimerStart = 0.25f;
    private Vector3 _reticleLocation;
    private Vector3 _vectorToReticle;
    private Quaternion _startRotation;
    private float _timeElapsed;
    private float _signedAngle;

    // Start is called before the first frame update
    void Start()
    {
        _text.text = "x" + _ammo;
        _timeElapsed = _lerpSpeed;
        _rotateTimer = _rotateTimerStart;
    }

    // Update is called once per frame
    void Update()
    {
        _reticleLocation = _reticle.transform.position;
        _rotateTimer -= Time.deltaTime;

        // Calculate rotation on timer
        if (_rotateTimer <= 0)
        {
            CalculateRotation();
        }

        // Call Rotation method on timer
        if (_timeElapsed <= _lerpSpeed)
        {
            RotateTowardsTarget(_vectorToReticle);
        }
            
    }

    /// <summary>
    /// Calculates the rotation needed for the lerp.
    /// </summary>
    private void CalculateRotation()
    {
        // Reset time elapsed
        _timeElapsed = 0;
        // Store the vector to the reticle's location
        _vectorToReticle = _reticleLocation - _turretTransform.position;

        // Store the current rotation of the turret
        _startRotation = _turretTransform.rotation;

        // Calculate angle needed to rotate to
        _signedAngle = Vector2.SignedAngle(transform.up, _vectorToReticle);

        // Reset _rotateTimer
        _rotateTimer = 0.25f;
    }

    /// <summary>
    /// Returns whether the turret can fire a Rocket or not.
    /// </summary>
    /// <returns>True if the turret can fire a Rocket, false if not.</returns>
    public bool CanFireRocket()
    {
        return _isAlive && _ammo > 0;
    }

    /// <summary>
    /// Returns if the turret is alive or not.
    /// </summary>
    /// <returns>True if still alive, false if not.</returns>
    public bool IsAlive()
    {
        return _isAlive;
    }

    /// <summary>
    /// Fires a Rocket from the turret
    /// </summary>
    public void FireRocket()
    {
        //Debug.Log(_ammo);
        float signedAngle = Vector2.SignedAngle(transform.up, _vectorToReticle);

        _projectile.GetComponent<PlayerRocket>().TargetPosition = _reticleLocation;

        Instantiate(_projectile, _cannonEnd.transform.position, Quaternion.Euler(0, 0, signedAngle));
        _ammo--;
        _text.text = "x" + _ammo;
    }

    /// <summary>
    /// Rotates the game object towards the target.
    /// </summary>
    /// <param name="vectorToTarget">The vector from the gameObject to the target.</param>
    private void RotateTowardsTarget(Vector3 vectorToTarget)
    {
        // Update the rotation to the rotation of the lerp at it's current state. An animation curve is passed in with the time elapsed determining the value of the curve.
        _turretTransform.rotation = Quaternion.Slerp(_startRotation, Quaternion.Euler(0, 0, _signedAngle), _curve.Evaluate(_timeElapsed / _lerpSpeed));

        // Increase _timeElapsed each frame
        _timeElapsed += Time.deltaTime;
    }

    /// <summary>
    /// Called when the turret is hit by an enemy Rocket, or an explosion.
    /// </summary>
    public void Hit()
    {
        _isAlive = false;
        _sprite.SetActive(false);
        _text.enabled = false;
    }

    /// <summary>
    /// Returns the amount of ammo remaining.
    /// </summary>
    /// <returns>Integer amount of ammo remaining.</returns>
    public int GetAmmoRemaining()
    {
        return _ammo;
    }
}
