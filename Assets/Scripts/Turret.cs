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

    private bool isAlive = true;
    private float _lerpSpeed = 0.5f;
    private float _rotateTimer = 0.05f;
    private Vector3 _reticleLocation;
    private Vector3 _vectorToReticle;

    // Start is called before the first frame update
    void Start()
    {
        _text.text = "x" + _ammo;
    }

    // Update is called once per frame
    void Update()
    {
        _reticleLocation = _reticle.transform.position;
        _vectorToReticle = _reticleLocation - _turretTransform.position;

        _rotateTimer -= Time.deltaTime;

        if (_rotateTimer <= 0)
        {
            RotateTowardsTarget(_reticleLocation, _vectorToReticle);
            _rotateTimer = 0.1f;
        }
    }

    public bool CanFireRocket()
    {
        return isAlive && _ammo > 0;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void FireRocket()
    {
        //Debug.Log(_ammo);
        float signedAngle = Vector2.SignedAngle(transform.up, _vectorToReticle);

        _projectile.GetComponent<PlayerRocket>().targetPosition = _reticleLocation;

        Instantiate(_projectile, _cannonEnd.transform.position, Quaternion.Euler(0, 0, signedAngle));
        _ammo--;
        _text.text = "x" + _ammo;
    }

    private void RotateTowardsTarget(Vector3 targetLocation, Vector3 vectorToTarget)
    {

        float timeElapsed = 0;
        Quaternion startRotation = _turretTransform.rotation;

        while (timeElapsed < _lerpSpeed)
        {
            float signedAngle = Vector2.SignedAngle(transform.up, vectorToTarget);
            _turretTransform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, signedAngle),
                _curve.Evaluate(timeElapsed));
            //Debug.Log(_curve.Evaluate(time) );
            timeElapsed += Time.deltaTime * _lerpSpeed;
        }
    }


    // Called when _reticle is destroyed
    public void Hit()
    {
        isAlive = false;
        _sprite.SetActive(false);
        _text.enabled = false;
    }

    public int GetAmmoRemaining()
    {
        return _ammo;
    }
}
