using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerWeaponAction : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSelector WeaponSelector;

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
        if (input.PlayerShootThisFrame() && WeaponSelector.ActiveWeapon != null)
        {
            WeaponSelector.ActiveWeapon.Shoot();
        }
    }
}
