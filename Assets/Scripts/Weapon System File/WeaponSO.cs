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

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootParticle;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(WeaponPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

        ShootParticle = Model.GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot()
    {
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            LastShootTime = Time.time;
            ShootParticle.Play();

            Vector3 shootDirection = ShootParticle.transform.forward
                + new Vector3(
                    Random.Range(
                        -ShootConfig.Spread.x,
                        ShootConfig.Spread.x),
                    Random.Range(
                        -ShootConfig.Spread.y,
                        ShootConfig.Spread.y),
                    Random.Range(
                        -ShootConfig.Spread.z,
                        ShootConfig.Spread.z)
                    );

            shootDirection.Normalize();

            if (Physics.Raycast(
                ShootParticle.transform.position,
                shootDirection,
                out RaycastHit hit,
                float.MaxValue,
                ShootConfig.HitMask))
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootParticle.transform.position,
                        hit.point,
                        hit));
            }
            else
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootParticle.transform.position,
                        ShootParticle.transform.position + (shootDirection * TrailConfig.MissDistance),
                        new RaycastHit()));

            }
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null;

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
