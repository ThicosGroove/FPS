using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class PlayerWeaponSelector : MonoBehaviour
{
    [Space]
    [Header("Initial Weapon parameters")]
    [SerializeField] private WeaponType WeaponType;
    [SerializeField] private Transform WeaponParent;
    [SerializeField] private List<GameObject> WeaponsList;

    [Space]
    [Header("Runtime Filled")]
    [SerializeField] public GameObject activeWeapon;

    private Dictionary<WeaponType, List<GameObject>> _dicWeapons = new Dictionary<WeaponType, List<GameObject>>();
    private List<GameObject> _listWeapon = new List<GameObject>();

    private WeaponBehaviour _activeWeaponBehaviour;
    private PlayerWeaponAction _playerWeaponAction;

    private PlayerInput _input;
    private int _currentWeaponIndex = 0;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _playerWeaponAction = GetComponent<PlayerWeaponAction>();

        activeWeapon = WeaponsList.Find(currentWeapon => currentWeapon.GetComponent<WeaponBehaviour>().MyWeaponSO.Type == WeaponType);

        if (activeWeapon == null)
        {
            Debug.LogError($"No WeaponSO found for WeaponType: {activeWeapon}");
            return;
        }

        _activeWeaponBehaviour = activeWeapon.GetComponent<WeaponBehaviour>();

        Spawn(WeaponParent);
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
            Debug.LogWarning("Troca de arma que já existe");

            StartCoroutine(_activeWeaponBehaviour.SwitchOutCO());

            activeWeapon = WeaponsList[_currentWeaponIndex];
            _activeWeaponBehaviour = activeWeapon.GetComponent<WeaponBehaviour>();
            _playerWeaponAction.UpdateCurrentWeapon(activeWeapon);

            StartCoroutine(_activeWeaponBehaviour.SwitchInCO());
        }
        else
        {
            Debug.LogWarning("Troca de arma que NÃO existe");
            StartCoroutine(_activeWeaponBehaviour.SwitchOutCO());

            activeWeapon = WeaponsList[_currentWeaponIndex];
            _activeWeaponBehaviour = activeWeapon.GetComponent<WeaponBehaviour>();

            Spawn(WeaponParent);

            _playerWeaponAction.UpdateCurrentWeapon(activeWeapon);
            // Como fazer uma coroutine começar só quando outra termina?
            StartCoroutine(_activeWeaponBehaviour.SwitchInCO());
        }
    }

    private bool GetWeapon(GameObject searchType)
    {
        var newWeapon = searchType.GetComponent<WeaponBehaviour>().MyWeaponSO;
   
        if (_dicWeapons.ContainsKey(newWeapon.Type) && _dicWeapons.ContainsValue(_listWeapon))
        {

            //activeWeapon = _listWeapon[];
            Debug.LogWarning("Achou arma igual");
            
            return true;
        }


        Debug.LogWarning("Não achou arma igual");
        return false;
    }

    public void Spawn(Transform Parent)
    {
        var newWeapon = Instantiate(activeWeapon);
        newWeapon.transform.SetParent(Parent, false);
        newWeapon.transform.localPosition = _activeWeaponBehaviour.MyWeaponSO.SpawnPoint;
        newWeapon.transform.localRotation = Quaternion.Euler(_activeWeaponBehaviour.MyWeaponSO.SpawnRotation);

        var weaponSO = newWeapon.GetComponent<WeaponBehaviour>().MyWeaponSO;

        activeWeapon = newWeapon;
        _activeWeaponBehaviour = activeWeapon.GetComponent<WeaponBehaviour>();

        //_weaponCreated.Add(newWeapon.name, weaponSO.Type);
        _listWeapon.Add(activeWeapon);
        _dicWeapons[weaponSO.Type] = _listWeapon;
    }
}
