using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

// Weapon System copied from https://www.youtube.com/watch?v=E-vIMamyORg&t=309s
[CreateAssetMenu(fileName ="Weapon", menuName = "Weapons/New Weapon", order = 1)]
public class WeaponSO : ScriptableObject
{
    public GameObject WeaponPrefab;
    public string Name;
    public WeaponType Type;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;

    public ShootConfigurationSO ShootConfig;
    public TrailConfigurationSO TrailConfig;

    [Range(0, 1)] public float timeToSwitchIn;
    [Range(0, 1)] public float timeToSwitchOut;

    [HideInInspector] public MonoBehaviour ActiveMonoBehaviour;
    [HideInInspector] public GameObject Model;
    [HideInInspector] public float LastShootTime;
    [HideInInspector] public ParticleSystem ShootParticle;
    [HideInInspector] public ObjectPool<TrailRenderer> TrailPool;


}
