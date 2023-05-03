using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootBehaviour : MonoBehaviour
{
    [SerializeField] private WeaponSO MyWeaponSO;
    [SerializeField] private ParticleBehaviour _particleBehaviour;
    [SerializeField] private LayerMask hitMask;

    [Space]
    [Header("Combo parameters")]
    [SerializeField] private float _timeLimitToComboSeconds = 1f;
    private Coroutine _comboCoroutine;
    private bool _isSecondShoot = false;
    private bool _isThirdShoot = false;

    private float _damage;
    private float _maxDistance = 1000f;

    private void Start()
    {
        _damage = MyWeaponSO.WeaponBaseDamage;
    }


    // Multiplicar Damage e Size
    public void ProcessShoot(Vector3 pos, Vector3 dir, float sizeMulti, float damageMulti)
    {     
        RaycastHit hitInfo;

        if (Physics.Raycast(pos, dir, out hitInfo, _maxDistance, hitMask))
        {
            _particleBehaviour.gameObject.transform.LookAt(hitInfo.point);

            ComboShot(sizeMulti);
        }
    }

    private void ComboShot(float sizeMulti)
    {
        var comboDamage =  _isThirdShoot ? _damage * 2.5f : _isSecondShoot ? _damage * 1.5f : _damage;

        Shoot(sizeMulti, comboDamage);
    }

    private void Shoot(float sizeMulti, float damageMulti)
    {
        _particleBehaviour.ParticlePlay(sizeMulti, damageMulti);

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
