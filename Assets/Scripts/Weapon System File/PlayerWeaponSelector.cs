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

    private WeaponBehaviour _myWeapon;
    private InputManager _input;
    private WeaponSO _currentWeapon;
    private int _currentWeaponIndex = 0;

    private void Start()
    {
        _input = InputManager.Instance;

        _currentWeapon = WeaponsList.Find(currentWeapon => currentWeapon.Type == Weapon);

        if (_currentWeapon == null)
        {
            Debug.LogError($"No WeaponSO found for WeaponType: {_currentWeapon}");
            return;
        }

        WeaponCreated.Add(_currentWeapon.Name, _currentWeapon.Type);

        ActiveWeapon = _currentWeapon;
        _myWeapon = ActiveWeapon.WeaponPrefab.GetComponent<WeaponBehaviour>();
        _myWeapon.Spawn(WeaponParent, this);
    }

    private void Update()
    {
        if (_input.PlayerChangeWeaponNext())
        {        
            ++_currentWeaponIndex;
            if (_currentWeaponIndex > (WeaponsList.Count - 1))
            {
                _currentWeaponIndex = 0;
            }

            Switching();
        }
    }

    private void Switching()
    {
        if (GetWeapon(WeaponsList[_currentWeaponIndex]))
        {
            StartCoroutine(_myWeapon.SwitchOutCO());
            ActiveWeapon = WeaponsList[_currentWeaponIndex];
            _currentWeapon = ActiveWeapon;
            _myWeapon = ActiveWeapon.WeaponPrefab.GetComponent<WeaponBehaviour>();
            StartCoroutine(_myWeapon.SwitchInCO());

        }
        else
        {
            StartCoroutine(_myWeapon.SwitchOutCO());
            ActiveWeapon = WeaponsList[_currentWeaponIndex];
            _myWeapon = ActiveWeapon.WeaponPrefab.GetComponent<WeaponBehaviour>();
            _myWeapon.Spawn(WeaponParent, this);
            _currentWeapon = ActiveWeapon;
            WeaponCreated.Add(_currentWeapon.Name, _currentWeapon.Type);
            StartCoroutine(_myWeapon.SwitchInCO());

        }
    }

    private bool GetWeapon(WeaponSO searchType)
    {
        if (WeaponCreated.ContainsValue(searchType.Type) && WeaponCreated.ContainsKey(searchType.Name))
            return true;

        return false;
    }
}
