using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    //public bool isAiming => input.GetPlayerAim();

    //[SerializeField] private WeaponSO weaponSO;

    //private InputManager input;
    //private Camera playerCamera;

    //private float defaultFOV;
    //private Coroutine zoomRoutine;

    //public bool zoomPressed = false;
    //private float time;
    //private bool canFire = false;

    //void Start()
    //{
    //    playerCamera = Camera.main;

    //    input = InputManager.Instance;
    //    defaultFOV = playerCamera.fieldOfView;

    //    transform.localPosition = weaponSO.defaultPos.position;
    //    transform.localRotation = weaponSO.defaultPos.rotation;
    //    transform.localScale = weaponSO.defaultPos.localScale;
    //}

    //void Update()
    //{
    //    FireRateHandle();
    //    if (input.PlayerShootThisFrame() && canFire)
    //        Shoot();

    //    if (weaponSO.canZoom)
    //        HandleZoom();
    //}

    //private void FireRateHandle()
    //{
    //    time += Time.deltaTime;

    //    float nextTimeToFire = 1 / weaponSO.fireRate;

    //    if (time >= nextTimeToFire)
    //    {
    //        canFire = true;
    //        time = 0;
    //    }
    //    else canFire = false;
    //}

    //private void Shoot()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, weaponSO.range))
    //    {
    //        Debug.LogWarning($"Hit {hit.collider.name}");
    //    }

    //    if (weaponSO.muzzleFlash != null)
    //    {
    //        weaponSO.muzzleFlash.Play();
    //    }
    //}

    //private void HandleZoom()
    //{
    //    if (!weaponSO.HoldToZoom)
    //    {
    //        if (isAiming)
    //        {
    //            if (!zoomPressed)
    //            {
    //                zoomPressed = true;
    //                if (zoomRoutine != null)
    //                {
    //                    StopCoroutine(zoomRoutine);
    //                    zoomRoutine = null;
    //                }
    //                zoomRoutine = StartCoroutine(ToggleZoom(zoomPressed));
    //            }
    //            else
    //            {
    //                zoomPressed = false;
    //                if (zoomRoutine != null)
    //                {
    //                    StopCoroutine(zoomRoutine);
    //                    zoomRoutine = null;
    //                }
    //                zoomRoutine = StartCoroutine(ToggleZoom(zoomPressed));
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (isAiming)
    //        {
    //            if (zoomRoutine != null)
    //            {
    //                StopCoroutine(zoomRoutine);
    //                zoomRoutine = null;
    //            }
    //            zoomRoutine = StartCoroutine(ToggleZoom(true));
    //        }

    //        if (isAiming)
    //        {
    //            if (zoomRoutine != null)
    //            {
    //                StopCoroutine(zoomRoutine);
    //                zoomRoutine = null;
    //            }
    //            zoomRoutine = StartCoroutine(ToggleZoom(false));
    //        }
    //    }
    //}

    //private IEnumerator ToggleZoom(bool IsEnter)
    //{
    //    Vector3 targetPos = IsEnter ? weaponSO.AimPos.localPosition : weaponSO.defaultPos.localPosition;
    //    Quaternion targetRot = IsEnter ? weaponSO.AimPos.localRotation : weaponSO.defaultPos.localRotation;

    //    float targetFOV = IsEnter ? weaponSO.zoomFOV : defaultFOV;
    //    float startingFOV = playerCamera.fieldOfView;
    //    float timeElapsed = 0;

    //    while (timeElapsed < weaponSO.timeToZoom)
    //    {
    //        weaponSO.gun.transform.localPosition = Vector3.Lerp(weaponSO.gun.transform.localPosition, targetPos, timeElapsed / weaponSO.timeToZoom);
    //        weaponSO.gun.transform.localRotation = Quaternion.Lerp(weaponSO.gun.transform.localRotation, targetRot, timeElapsed / weaponSO.timeToZoom);

    //        playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / weaponSO.timeToZoom);

    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    playerCamera.fieldOfView = targetFOV;
    //    zoomRoutine = null;
    //}
}
