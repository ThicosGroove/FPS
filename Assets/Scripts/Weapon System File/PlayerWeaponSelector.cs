using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class PlayerWeaponSelector : MonoBehaviour
{
    [SerializeField] private WeaponType Weapon;
    [SerializeField] private Transform WeaponParent;
    [SerializeField] private List<WeaponSO> WeaponsList;

    [Space]
    [Header("Runtime Filled")]
    public WeaponSO ActiveWeapon;



    private void Start()
    {
        WeaponSO weapon = WeaponsList.Find(weapon => weapon.Type == Weapon);

        if (weapon == null)
        {
            Debug.LogError($"No WeaponSO found for WeaponType: {weapon}");
            return;
        }

        ActiveWeapon = weapon;
        weapon.Spawn(WeaponParent, this);
    }
}
