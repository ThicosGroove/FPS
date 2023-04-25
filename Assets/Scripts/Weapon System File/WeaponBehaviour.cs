using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private WeaponSO myWeapon;

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
        yield return new WaitForSeconds(myWeapon.timeToSwitchIn);
        myWeapon.Model.gameObject.SetActive(true);
    }

    public IEnumerator SwitchOut()
    {
        yield return new WaitForSeconds(myWeapon.timeToSwitchOut);
        myWeapon.Model.gameObject.SetActive(false);
    }


    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        myWeapon.ActiveMonoBehaviour = ActiveMonoBehaviour;
        myWeapon.LastShootTime = 0;
        myWeapon.TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        myWeapon.Model = Instantiate(myWeapon.WeaponPrefab);
        myWeapon.Model.transform.SetParent(Parent, false);
        myWeapon.Model.transform.localPosition = myWeapon.SpawnPoint;
        myWeapon.Model.transform.localRotation = Quaternion.Euler(myWeapon.SpawnRotation);

        myWeapon.ShootParticle = myWeapon.Model.GetComponentInChildren<ParticleSystem>();
    }

    private Transform CameraPosition()
    {
        return myWeapon.Model.GetComponentInParent<Camera>().transform;
    }

    public void Shoot()
    {
        if (Time.time > myWeapon.ShootConfig.FireRate + myWeapon.LastShootTime)
        {
            myWeapon.LastShootTime = Time.time;
            myWeapon.ShootParticle.Play();

            Vector3 shootDirection = CameraPosition().forward
                + new Vector3(
                    Random.Range(
                        -myWeapon.ShootConfig.Spread.x,
                        myWeapon.ShootConfig.Spread.x),
                    Random.Range(
                        -myWeapon.ShootConfig.Spread.y,
                        myWeapon.ShootConfig.Spread.y),
                    Random.Range(
                        -myWeapon.ShootConfig.Spread.z,
                        myWeapon.ShootConfig.Spread.z)
                    );

            shootDirection.Normalize();

            if (Physics.Raycast(
                myWeapon.ShootParticle.transform.position,
                shootDirection,
                out RaycastHit hit,
                float.MaxValue,
                myWeapon.ShootConfig.HitMask))
            {
                myWeapon.ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        myWeapon.ShootParticle.transform.position,
                        hit.point,
                        hit));

                Debug.LogWarning(hit.transform.gameObject.name);

                AEnemy enemy = hit.transform.gameObject.GetComponent<AEnemy>();

                if (enemy != null)
                {
                    enemy.LostHealth(myWeapon.WeaponBaseDamage);
                }

            }
            else
            {
                myWeapon.ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        myWeapon.ShootParticle.transform.position,
                        myWeapon.ShootParticle.transform.position + (shootDirection * myWeapon.TrailConfig.MissDistance),
                        new RaycastHit()));
            }
        }
    }


    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance = myWeapon.TrailPool.Get();
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
            remainingDistance -= myWeapon.TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        yield return new WaitForSeconds(myWeapon.TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        myWeapon.TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = myWeapon.TrailConfig.Color;
        trail.material = myWeapon.TrailConfig.Material;
        trail.widthCurve = myWeapon.TrailConfig.WidthCurve;
        trail.time = myWeapon.TrailConfig.Duration;
        trail.minVertexDistance = myWeapon.TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
