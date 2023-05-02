using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class PlayerWeaponAction : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSelector WeaponSelector;
    [SerializeField] private float minChargedShoot = 0.5f; // minimo que precisa carregar 
    [SerializeField] private float timerThreshold = 2f;   // maximo de tempo que segura o tiro
    
    [SerializeField] private GameObject _myWeapon;
    private WeaponShootBehaviour _weaponShootBehaviour;

    private float _timer;
    private float _sizeMultiplier;
    private float _damageMultiplier;

    private Camera _mainCamera;
    private Vector3 _cameraPos;
    private Vector3 _cameraDirection;

    PlayerInput _input;

    void Start()
    {
        _input = GetComponent<PlayerInput>();

        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _myWeapon = WeaponSelector.ActiveWeapon;
        _weaponShootBehaviour = _myWeapon.GetComponent<WeaponShootBehaviour>();
    }

    void Update()
    {
        ChargedShootBehaviour();
    }


    private void ChargedShootBehaviour()
    {
        if (_input.IsShootCharging)
        {
            _timer += Time.deltaTime;

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

        _damageMultiplier = 0;
        _sizeMultiplier = 0;
        _timer = 0;

        ShootDirection();
        _weaponShootBehaviour.ProcessShoot(_cameraPos, _cameraDirection, _sizeMultiplier, _damageMultiplier);
    }

    private void ShootDirection()
    {
        _cameraPos = _mainCamera.transform.position;
        _cameraDirection = _mainCamera.transform.TransformDirection(Vector3.forward);
    }  
}
