using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class PlayerWeaponAction : MonoBehaviour
{
    [Space]
    [SerializeField] private float minChargedShoot = 0.5f; // minimo que precisa carregar 
    [SerializeField] private float timerThreshold = 2f;   // maximo de tempo que segura o tiro

    [Space]
    [Header("Runtime Filled")]
    [SerializeField] private GameObject _currentWeapon;

    private PlayerWeaponSelector _weaponSelector;
    private WeaponShootBehaviour _weaponShootBehaviour;
    private WeaponBehaviour _weaponBehaviour;
    private PlayerInput _input;

    private float _timer;
    private float _sizeMultiplier;
    private float _holdPowerMultiplier;

    private Camera _mainCamera;
    private Vector3 _cameraPos;
    private Vector3 _cameraDirection;

    void Start()
    {
        _input = GetComponent<PlayerInput>();
        _weaponSelector = GetComponent<PlayerWeaponSelector>();

        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _currentWeapon = _weaponSelector.activeWeapon;
        _weaponShootBehaviour = _currentWeapon.GetComponent<WeaponShootBehaviour>();
        _weaponBehaviour = _currentWeapon.GetComponent<WeaponBehaviour>();
    }

    void Update()
    {
        ChargedShootBehaviour();
    }


    public void UpdateCurrentWeapon(GameObject currentWeapon)
    {
        _currentWeapon = currentWeapon;
        _weaponShootBehaviour = _currentWeapon.GetComponent<WeaponShootBehaviour>();
    }

    private void ChargedShootBehaviour()
    {
        if (_input.IsShootCharging)
        {
            _timer += Time.deltaTime;

            if (_timer > minChargedShoot) _weaponBehaviour.StartChargingParticles();
         

            if (_timer >= timerThreshold)
            {
                ShootBehaviour();
            }
        }
        else if (_input.IsShootGoOff && _timer > minChargedShoot)
        {
            ShootBehaviour();        
        }
    }

    private void ShootBehaviour()
    {
        _input.IsShootCharging = false;
        _input.IsShootStarted = false;
        _input.IsShootGoOff = false;

        _weaponBehaviour.StopChargingParticles();

        _holdPowerMultiplier = _timer;
        _sizeMultiplier = _timer;
        _timer = 0;

        ShootDirection();

        _weaponShootBehaviour.ProcessShoot(_cameraPos, _cameraDirection, _sizeMultiplier, _holdPowerMultiplier);
    }

    private void ShootDirection()
    {
        _cameraPos = _mainCamera.transform.position;
        _cameraDirection = _mainCamera.transform.TransformDirection(Vector3.forward);
    }  
}
