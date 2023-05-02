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
    
    private WeaponBehaviour _myWeapon;
    private float _timer;
    private float _sizeMultiplier;
    private float _damageMultiplier;

    PlayerInput _input;

    void Start()
    {
        _input = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _myWeapon = GetComponentInChildren<WeaponBehaviour>().gameObject;
        _weaponShootBehaviour = _myWeapon.GetComponent<WeaponShootBehaviour>();
    }

    void Update()
    {
        //if (WeaponSelector.ActiveWeapon == null) return;



        ChargedShootBehaviour();

    }


    private void ChargedShootBehaviour()
    {
        if (_input.IsShootCharging)
        {
            _timer += Time.deltaTime;

            if (_timer >= timerTreshold)
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

        _weaponShootBehaviour.ProcessShoot(_sizeMultiplier, _damageMultiplier);
    }
}
