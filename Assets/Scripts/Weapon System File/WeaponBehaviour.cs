using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] public WeaponSO myWeaponSO;

    private Animator anim;
    private InputManager input;

    private void Awake()
    {

    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        input = InputManager.Instance;
    }

    private void Update()
    {
        if (input.PlayerChangeWeaponNext())
        {
            PlaySwithOutAnim();
        }
    }

    public void PlaySwithOutAnim()
    {
        anim.SetBool("hasChanged", true);
    }


    public IEnumerator SwitchIn()
    {
        yield return new WaitForSeconds(myWeaponSO.timeToSwitchIn);
        myWeaponSO.Model.gameObject.SetActive(true);
    }

    public IEnumerator SwitchOut()
    {
        yield return new WaitForSeconds(myWeaponSO.timeToSwitchOut);
        myWeaponSO.Model.gameObject.SetActive(false);
    }


    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        myWeaponSO.ActiveMonoBehaviour = ActiveMonoBehaviour;
        myWeaponSO.LastShootTime = 0;
        myWeaponSO.TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        myWeaponSO.Model = Instantiate(myWeaponSO.WeaponPrefab);
        myWeaponSO.Model.transform.SetParent(Parent, false);
        myWeaponSO.Model.transform.localPosition = myWeaponSO.SpawnPoint;
        myWeaponSO.Model.transform.localRotation = Quaternion.Euler(myWeaponSO.SpawnRotation);

        myWeaponSO.ShootParticle = myWeaponSO.Model.GetComponentInChildren<ParticleSystem>();
    }

    private Transform CameraPosition()
    {
        return myWeaponSO.Model.GetComponentInParent<Camera>().transform;
    }

    public void Shoot(float size, float damageMultiplier)
    {
        if (Time.time > myWeaponSO.ShootConfig.FireRate + myWeaponSO.LastShootTime)
        {

            Debug.LogWarning(myWeaponSO.WeaponBaseDamage);

            myWeaponSO.LastShootTime = Time.time;
            myWeaponSO.ShootParticle.Play();

            Vector3 shootDirection = CameraPosition().forward
                + new Vector3(
                    Random.Range(
                        -myWeaponSO.ShootConfig.Spread.x,
                        myWeaponSO.ShootConfig.Spread.x),
                    Random.Range(
                        -myWeaponSO.ShootConfig.Spread.y,
                        myWeaponSO.ShootConfig.Spread.y),
                    Random.Range(
                        -myWeaponSO.ShootConfig.Spread.z,
                        myWeaponSO.ShootConfig.Spread.z)
                    );

            shootDirection.Normalize();

            if (Physics.Raycast(
                myWeaponSO.ShootParticle.transform.position,
                shootDirection,
                out RaycastHit hit,
                float.MaxValue,
                myWeaponSO.ShootConfig.HitMask))
            {
                myWeaponSO.ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        myWeaponSO.ShootParticle.transform.position,
                        hit.point,
                        hit));

                Debug.LogWarning(hit.transform.gameObject.name);

                AEnemy enemy = hit.transform.gameObject.GetComponent<AEnemy>();

                if (enemy != null)
                {
                    enemy.LostHealth(myWeaponSO.WeaponBaseDamage);
                }

            }
            else
            {
                myWeaponSO.ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        myWeaponSO.ShootParticle.transform.position,
                        myWeaponSO.ShootParticle.transform.position + (shootDirection * myWeaponSO.TrailConfig.MissDistance),
                        new RaycastHit()));
            }
        }
    }


    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance = myWeaponSO.TrailPool.Get();
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
            remainingDistance -= myWeaponSO.TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        yield return new WaitForSeconds(myWeaponSO.TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        myWeaponSO.TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = myWeaponSO.TrailConfig.Color;
        trail.material = myWeaponSO.TrailConfig.Material;
        trail.widthCurve = myWeaponSO.TrailConfig.WidthCurve;
        trail.time = myWeaponSO.TrailConfig.Duration;
        trail.minVertexDistance = myWeaponSO.TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
