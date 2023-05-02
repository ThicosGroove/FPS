using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootBehaviour : MonoBehaviour
{
    [SerializeField] private WeaponSO MyWeaponSO;
    [SerializeField] private ParticleBehaviour _particleBehaviour;

    [SerializeField] private LayerMask hitMask;

    [SerializeField] Camera mainCamera;

    private float damage;

    private void Start()
    {
        //mainCamera = Camera.main;

        damage = MyWeaponSO.WeaponBaseDamage;
    }


    // Multiplicar Damage e Size
    public void ProcessShoot(Vector3 pos, Vector3 dir, float sizeMulti, float damageMulti)
    {
      

        RaycastHit hitInfo;

        if (Physics.Raycast(pos, dir, out hitInfo, 1000f, hitMask))
        {
            Debug.LogWarning(hitInfo.point);

            _particleBehaviour.gameObject.transform.LookAt(hitInfo.point);

            Shoot(sizeMulti, damage);
        }
    }

    private void Shoot(float sizeMulti, float damageMulti)
    {
        _particleBehaviour.ParticlePlay(sizeMulti, damageMulti);
    }
}
