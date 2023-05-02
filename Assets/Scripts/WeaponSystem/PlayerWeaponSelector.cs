using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class PlayerWeaponSelector : MonoBehaviour
{
    [SerializeField] private WeaponType Weapon;
    [SerializeField] private Transform WeaponParent;
    [SerializeField] private List<GameObject> WeaponsList;
    [SerializeField] private Dictionary<string, WeaponType> WeaponCreated = new Dictionary<string, WeaponType>();

    [Space]
    [Header("Runtime Filled")]
    [SerializeField] public GameObject ActiveWeapon;

    private WeaponSO _weaponSO;

    private WeaponBehaviour _myWeaponBehaviour;
    private PlayerInput _input;
    private int _currentWeaponIndex = 0;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();

        //ActiveWeapon = WeaponsList.Find(currentWeapon => currentWeapon.Type == Weapon);
        ActiveWeapon = WeaponsList[0].gameObject;

        if (ActiveWeapon == null)
        {
            Debug.LogError($"No WeaponSO found for WeaponType: {ActiveWeapon}");
            return;
        }

        _weaponSO = ActiveWeapon.GetComponent<WeaponBehaviour>().MyWeaponSO;

        _myWeaponBehaviour = _weaponSO.WeaponPrefab.GetComponent<WeaponBehaviour>();

        
        Spawn(WeaponParent, this);
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
            StartCoroutine(_myWeaponBehaviour.SwitchOutCO());
            ActiveWeapon = WeaponsList[_currentWeaponIndex];
            _myWeaponBehaviour = _weaponSO.WeaponPrefab.GetComponent<WeaponBehaviour>();
            StartCoroutine(_myWeaponBehaviour.SwitchInCO());

        }
        else
        {
            StartCoroutine(_myWeaponBehaviour.SwitchOutCO());
            ActiveWeapon = WeaponsList[_currentWeaponIndex];
            _myWeaponBehaviour = _weaponSO.WeaponPrefab.GetComponent<WeaponBehaviour>();
            Spawn(WeaponParent, this);
            WeaponCreated.Add(_weaponSO.Name, _weaponSO.Type);
            StartCoroutine(_myWeaponBehaviour.SwitchInCO());

        }
    }

    private bool GetWeapon(GameObject searchType)
    {
        var newWeapon = searchType.GetComponent<WeaponBehaviour>().MyWeaponSO;

        if (WeaponCreated.ContainsValue(newWeapon.Type) && WeaponCreated.ContainsKey(newWeapon.Name))
            return true;

        return false;
    }

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {

        var newWeapon = Instantiate(ActiveWeapon);
        newWeapon.transform.SetParent(Parent, false);
        newWeapon.transform.localPosition = _weaponSO.SpawnPoint;
        newWeapon.transform.localRotation = Quaternion.Euler(_weaponSO.SpawnRotation);

        ActiveWeapon = newWeapon;
        WeaponCreated.Add(_weaponSO.name, _weaponSO.Type);
    }
}
