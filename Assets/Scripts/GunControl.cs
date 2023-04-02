using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
public class GunControl : MonoBehaviour
{
    Camera playerCamera;

    [Header("Gun Parameters")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 10f;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem muzzleFlash;

    private InputControl input;

    private float time;
    private bool canFire = false;

    private void Awake()
    {
        input = new InputControl();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private bool PlayerShootThisFrame()
    {
        float shoot = input.Character.Shoot.ReadValue<float>();
        return shoot > 0 ? true : false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        FireRateHandle();
        if (PlayerShootThisFrame() && canFire)       
            Shoot();        
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
}
