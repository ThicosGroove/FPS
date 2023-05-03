using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootBehaviour : MonoBehaviour
{
    [SerializeField] private WeaponBehaviour _weaponBehaviour;

    [Space]
    [SerializeField] private WeaponSO MyWeaponSO;
    [SerializeField] private ParticleBehaviour _particleBehaviour;
    [SerializeField] private LayerMask hitMask;

    [Space]
    [Header("Combo parameters")]
    [SerializeField] private float _timeLimitToComboSeconds = 3f;
    [SerializeField] private float _thirdShotMultiplier = 2f;
    [SerializeField] private float _secondShotMultiplier = 1.5f;
    private Coroutine _comboCoroutine;
    private bool _isSecondShoot = false;
    private bool _isThirdShoot = false;

    public bool IsSecondShoot { get => _isSecondShoot; private set { _isSecondShoot = value; } }
    public bool IsThirdShoot { get => _isThirdShoot; private set { _isThirdShoot = value; } }

    private float _baseDamage;
    private float _maxDistance = 1000f;


    private void Start()
    {
        _baseDamage = MyWeaponSO.WeaponBaseDamage;
    }


    // Multiplicar Damage e Size
    public void ProcessShoot(Vector3 pos, Vector3 dir, float sizeMulti, float holdPower)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(pos, dir, out hitInfo, _maxDistance, hitMask))
        {
            _particleBehaviour.gameObject.transform.LookAt(hitInfo.point);

            ComboShot(sizeMulti, holdPower);
        }
    }

    private void ComboShot(float sizeMulti, float holdPower)
    {
        //Debug.LogWarning($"Final damage = {finalDamage}, Combo damage = {comboDamage}, Hold Power = {holdPower}");

        Shoot(CalculateFinalSize(sizeMulti), CalculateFinalDamage(holdPower));
    }

    private float CalculateFinalDamage(float holdPower)
    {
        var thirdShot = _baseDamage * _thirdShotMultiplier;
        var secondShot = _baseDamage * _secondShotMultiplier;

        var comboDamage = _isThirdShoot ? thirdShot : _isSecondShoot ? secondShot : _baseDamage;
        return comboDamage * holdPower;
    }

    private float CalculateFinalSize(float sizeMulti)
    {
        var secondShot = 2f;
        var thirdShot = 3f;

        var comboSize = _isThirdShoot ? thirdShot : _isSecondShoot ? secondShot : 1;

        return sizeMulti * comboSize;
    }

    private void Shoot(float sizeMulti, float finalDamage)
    {
        _particleBehaviour.ParticlePlay(sizeMulti, finalDamage);

        if (_comboCoroutine != null) StopCoroutine(_comboCoroutine);
        _comboCoroutine = StartCoroutine(ComboTimeCO());
    }

    IEnumerator ComboTimeCO()
    {
        if (_isSecondShoot && !_isThirdShoot) _isThirdShoot = true;

        else _isSecondShoot = true;

        yield return new WaitForSeconds(_timeLimitToComboSeconds);

        _isSecondShoot = false;
        _isThirdShoot = false;
    }


}
