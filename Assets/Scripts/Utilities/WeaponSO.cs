using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Gun")]
public class WeaponSO : ScriptableObject
{
    [Header("Nessessary Obj")]
    [SerializeField] public GameObject gun;

    [Header("Gun Parameters")]
    [SerializeField] public float damage = 0;
    [SerializeField] public float range = 0;
    [SerializeField] public float fireRate = 0;

    [Header("Zoom Parameters")]
    [SerializeField] public Transform defaultPos;
    [SerializeField] public Transform AimPos;
    [SerializeField] public bool canZoom = true;
    [SerializeField] public bool HoldToZoom = false;
    [SerializeField] public float timeToZoom = 0.3f;
    [SerializeField] public float zoomFOV = 30f;

    [Header("Particle System")]
    [SerializeField] public ParticleSystem muzzleFlash;
    [SerializeField] public ParticleSystem bullet;
}
