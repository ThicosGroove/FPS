using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerWeaponAction : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSelector WeaponSelector;
    [SerializeField] private float minChargedShoot; // minimo que precisa carregar 
    [SerializeField] private float timerTreshold;   // maximo de tempo que segura o tiro

    private WeaponBehaviour _myWeapon;
    private float _timer;
    private float _sizeMultiplier;
    private float _damageMultiplier;

    InputManager _input;

    void Start()
    {
        _input = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (WeaponSelector.ActiveWeapon == null) return;

        _myWeapon = WeaponSelector.ActiveWeapon.WeaponPrefab.GetComponent<WeaponBehaviour>();


        if (_input.IsShootStarted)
        {
            ChargedShootBehaviour();
        }
    }


    private void ChargedShootBehaviour()
    {
        if (_input.IsShootCharging)
        {
            // Começa a contar o timer
            // O timer é a Porcentagem do tiro
            // Aumenta o Dano 
            // Aumenta o Tamanho

            _timer += Time.deltaTime;

            //_damageMultiplier = _timer;   não tá funcionando da forma que eu queria
            //_sizeMultiplier = _timer;

            if (_timer >= timerTreshold)
            {
                //Atira sozinho depois do tempo limite
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
        // Para o Timer e Atira
        // usando o timer como porcentagem para o dano 
        // e o tamanho do tiro

        Debug.LogWarning(_myWeapon.MyWeaponSO.WeaponBaseDamage);
        _input.IsShootCharging = false;
        _input.IsShootStarted = false;
        _damageMultiplier = 0;
        _sizeMultiplier = 0;
        _timer = 0;
        _myWeapon.Shoot(_sizeMultiplier, _damageMultiplier); // Temporário, até entender como funciona o multiplicador
    }
}
