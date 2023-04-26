using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerWeaponAction : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSelector WeaponSelector;
    [SerializeField] private float minChargedShoot; // minimo que precisa carregar 
    [SerializeField] private float timerTreshold;   // maximo de tempo que segura o tiro

    private WeaponBehaviour myWeapon;
    private float timer;
    private float sizeMultiplier;
    private float damageMultiplier;


    InputManager input;



    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (WeaponSelector.ActiveWeapon == null) return;

        myWeapon = WeaponSelector.ActiveWeapon.WeaponPrefab.GetComponent<WeaponBehaviour>();


        if (input.IsShootStarted)
        {
            ChargedShootBehaviour();
        }
    }


    private void ChargedShootBehaviour()
    {
        if (input.IsShootCharging)
        {
            timer += Time.deltaTime;

            // Começa a contar o timer
            // O timer é a Porcentagem do tiro

            damageMultiplier = timer;
            sizeMultiplier = timer;

            // Aumenta o Dano 
            // Aumenta o Tamanho

            if (timer >= timerTreshold)
            {
                //Atira sozinho depois do tempo limite
                ShootBehaviour();
            }

        }
        else if (input.IsShootGoOff && timer > minChargedShoot)
        {
            ShootBehaviour();

            // Para o Timer e Atira
            // usando o timer como porcentagem para o dano 
            // e o tamanho do tiro
        }
    }

    private void ShootBehaviour()
    {
        Debug.LogWarning(myWeapon.myWeaponSO.WeaponBaseDamage);
        input.IsShootCharging = false;
        input.IsShootStarted = false;
        damageMultiplier = 0;
        sizeMultiplier = 0;
        timer = 0;
        myWeapon.Shoot(sizeMultiplier, damageMultiplier); // Temporário, até entender como funciona o multiplicador
    }
}
