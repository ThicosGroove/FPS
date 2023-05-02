using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootBehaviour : MonoBehaviour
{
    [SerializeField] private WeaponSO MyWeaponSO;
    [SerializeField] private ParticleBehaviour _particleBehaviour;

    [SerializeField] private LayerMask hitMask;

    Camera mainCamera;

    private float damage;

    [SerializeField] Transform FPSCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        damage = MyWeaponSO.WeaponBaseDamage;
    }


    // Multiplicar Damage e Size
    public void ProcessShoot(float sizeMulti, float damageMulti)
    {
        var cameraTransform = mainCamera.transform.position;
        var dir = mainCamera.transform.TransformDirection(Vector3.forward);

        RaycastHit hitInfo;

        if (Physics.Raycast(cameraTransform, dir, out hitInfo, 1000f, hitMask))
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
