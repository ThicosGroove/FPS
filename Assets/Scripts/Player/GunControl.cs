using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public bool isAiming => input.GetPlayerAim();

    [Header("Nessessary Obj")]
    [SerializeField] GameObject gun;
    [SerializeField] private Camera playerCamera;

    // Atualizar depois para mudar de arma usando Scriptable Objects
    [Header("Gun Parameters")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 10f;

    [Header("Zoom Parameters")]
    [SerializeField] private Transform defaultPos;
    [SerializeField] private Transform AimPos;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool HoldToZoom = false;
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 30f;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem bullet;

    private float defaultFOV;
    private Coroutine zoomRoutine;
    public bool zoomPressed = false;

    private InputManager input;

    private float time;
    private bool canFire = false;

    void Start()
    {
        input = InputManager.Instance;
        defaultFOV = playerCamera.fieldOfView;

        gun.transform.position = defaultPos.position;
        gun.transform.rotation = defaultPos.rotation;
    }

    void Update()
    {
        FireRateHandle();
        if (input.PlayerShootThisFrame() && canFire)       
            Shoot();

        if (canZoom)
            HandleZoom();
    }

    private void FireRateHandle()
    {
        time += Time.deltaTime;

        float nextTimeToFire = 1 / fireRate;

        if (time >= nextTimeToFire)
        {
            canFire = true;
            time = 0;
        }
        else canFire = false; 
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.LogWarning($"Hit {hit.collider.name}");
        }

        muzzleFlash.Play();
    }

    private void HandleZoom()
    {
        if (!HoldToZoom)
        {
            if (isAiming)
            {
                if (!zoomPressed)
                {
                    zoomPressed = true;
                    if (zoomRoutine != null)
                    {
                        StopCoroutine(zoomRoutine);
                        zoomRoutine = null;
                    }
                    zoomRoutine = StartCoroutine(ToggleZoom(zoomPressed));
                }
                else
                {
                    zoomPressed = false;
                    if (zoomRoutine != null)
                    {
                        StopCoroutine(zoomRoutine);
                        zoomRoutine = null;
                    }
                    zoomRoutine = StartCoroutine(ToggleZoom(zoomPressed));
                }
            }
        }
        else
        {
            if (isAiming)
            {
                if (zoomRoutine != null)
                {
                    StopCoroutine(zoomRoutine);
                    zoomRoutine = null;
                }
                zoomRoutine = StartCoroutine(ToggleZoom(true));
            }

            if (isAiming)
            {
                if (zoomRoutine != null)
                {
                    StopCoroutine(zoomRoutine);
                    zoomRoutine = null;
                }
                zoomRoutine = StartCoroutine(ToggleZoom(false));
            }
        }
    }

    private IEnumerator ToggleZoom(bool IsEnter)
    {
        Vector3 targetPos = IsEnter ? AimPos.localPosition : defaultPos.localPosition;
        Quaternion targetRot = IsEnter ? AimPos.localRotation : defaultPos.localRotation;

        float targetFOV = IsEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, targetPos, timeElapsed / timeToZoom);
            gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, targetRot, timeElapsed / timeToZoom);

            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }
}
