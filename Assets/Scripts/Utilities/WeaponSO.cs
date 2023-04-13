using UnityEngine;

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

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootParticle;
}
