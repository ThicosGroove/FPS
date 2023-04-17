using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class PlayerWeaponSelector : MonoBehaviour
{
    [SerializeField] private WeaponType Weapon;
    [SerializeField] private Transform WeaponParent;
    [SerializeField] private List<WeaponSO> WeaponsList;
    [SerializeField] private Dictionary<string, WeaponType> WeaponCreated = new Dictionary<string, WeaponType>();

    [Space]
    [Header("Runtime Filled")]
    public WeaponSO ActiveWeapon;

    private InputManager input;
    private WeaponSO currentWeapon;
    private int currentWeaponIndex = 0;

    private void Start()
    {
        input = InputManager.Instance;

        currentWeapon = WeaponsList.Find(currentWeapon => currentWeapon.Type == Weapon);

        if (currentWeapon == null)
        {
            Debug.LogError($"No WeaponSO found for WeaponType: {currentWeapon}");
            return;
        }

        WeaponCreated.Add(currentWeapon.Name, currentWeapon.Type);

        ActiveWeapon = currentWeapon;
        currentWeapon.Spawn(WeaponParent, this);

        Debug.LogWarning(WeaponsList.Count);
    }

    private void Update()
    {
        if (input.PlayerChangeWeapon())
        {
            ++currentWeaponIndex;

            if (currentWeaponIndex > (WeaponsList.Count - 1))
            {
                currentWeaponIndex = 0;
            }



            if (GetWeapon(WeaponsList[currentWeaponIndex]))
            {
                currentWeapon.SwitchOut();
                ActiveWeapon = WeaponsList[currentWeaponIndex];
                currentWeapon = ActiveWeapon;
                currentWeapon.SwitchIn();

            }
            else
            {
                currentWeapon.SwitchOut();
                ActiveWeapon = WeaponsList[currentWeaponIndex];
                ActiveWeapon.Spawn(WeaponParent, this);
                currentWeapon = ActiveWeapon;
                WeaponCreated.Add(currentWeapon.Name, currentWeapon.Type);
                currentWeapon.SwitchIn();

            }
        }
    }

    private bool GetWeapon(WeaponSO searchType)
    {
        if (WeaponCreated.ContainsValue(searchType.Type) && WeaponCreated.ContainsKey(searchType.Name))        
            return true;
        
        return false;
    }
}
