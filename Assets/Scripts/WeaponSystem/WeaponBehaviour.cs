using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] public WeaponSO MyWeaponSO;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        MyWeaponSO.ActiveMonoBehaviour = this;

        if (!_anim)
        {
            Debug.LogError("Não achou animator");
        }
    }

    public void PlaySwithOutAnim()
    {
        _anim.SetBool("hasChanged", true);
    }


    public IEnumerator SwitchInCO()
    {
        yield return new WaitForSeconds(MyWeaponSO.TimeToSwitchIn);
        //_anim.SetBool("hasChanged", true);
    }

    public IEnumerator SwitchOutCO()
    {
        Debug.LogWarning($"Trocou arma {this.name}");

        _anim.SetBool("hasChanged", true);
        yield return new WaitForSeconds(MyWeaponSO.TimeToSwitchOut);
    }

    #region Test
    //private Transform CameraPosition()
    //{
    //    return MyWeaponSO.Model.GetComponentInParent<Camera>().transform;
    //}

    // Calcular a direção do Raycast
    // Play Trail em todos os tiros
    // Usar o evento do particle system para saber o que acertou

    //public void Shoot(float size, float damageMultiplier)
    //{
    //    if (Time.time > MyWeaponSO.ShootConfig.FireRate + MyWeaponSO.LastShootTime)
    //    {

    //        Debug.LogWarning(MyWeaponSO.WeaponBaseDamage);

    //        MyWeaponSO.LastShootTime = Time.time;
    //        MyWeaponSO.ShootParticle.Play();

    //        Vector3 shootDirection = CameraPosition().forward
    //            + new Vector3(
    //                Random.Range(
    //                    -MyWeaponSO.ShootConfig.Spread.x,
    //                    MyWeaponSO.ShootConfig.Spread.x),
    //                Random.Range(
    //                    -MyWeaponSO.ShootConfig.Spread.y,
    //                    MyWeaponSO.ShootConfig.Spread.y),
    //                Random.Range(
    //                    -MyWeaponSO.ShootConfig.Spread.z,
    //                    MyWeaponSO.ShootConfig.Spread.z)
    //                );

    //        shootDirection.Normalize();


    //        if (Physics.Raycast(
    //            MyWeaponSO.ShootParticle.transform.position,
    //            shootDirection,
    //            out RaycastHit hit,
    //            float.MaxValue,
    //            MyWeaponSO.ShootConfig.HitMask))
    //        {
    //            MyWeaponSO.ActiveMonoBehaviour.StartCoroutine(
    //                PlayTrailCO(
    //                    MyWeaponSO.ShootParticle.transform.position,
    //                    hit.point,
    //                    hit));

    //            Debug.LogWarning(hit.transform.gameObject.name);

    //            AEnemy enemy = hit.transform.gameObject.GetComponent<AEnemy>();

    //            if (enemy != null)
    //            {
    //                enemy.LostHealth(MyWeaponSO.WeaponBaseDamage);
    //            }

    //        }
    //        else
    //        {
    //            MyWeaponSO.ActiveMonoBehaviour.StartCoroutine(
    //                PlayTrailCO(
    //                    MyWeaponSO.ShootParticle.transform.position,
    //                    MyWeaponSO.ShootParticle.transform.position + (shootDirection * MyWeaponSO.TrailConfig.MissDistance), // acertar a dire��o
    //                    new RaycastHit()));
    //        }
    //    }
    //}

    //private IEnumerator PlayTrailCO(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    //{
    //    TrailRenderer instance = MyWeaponSO.TrailPool.Get();
    //    instance.gameObject.SetActive(true);
    //    instance.transform.position = StartPoint;
    //    yield return null;

    //    instance.emitting = true;

    //    float distance = Vector3.Distance(StartPoint, EndPoint);
    //    float remainingDistance = distance;
    //    while (remainingDistance > 0)
    //    {
    //        instance.transform.position = Vector3.Lerp(
    //            StartPoint,
    //            EndPoint,
    //            Mathf.Clamp01(1 - (remainingDistance / distance)));
    //        remainingDistance -= MyWeaponSO.TrailConfig.SimulationSpeed * Time.deltaTime;

    //        yield return null;
    //    }

    //    instance.transform.position = EndPoint;

    //    yield return new WaitForSeconds(MyWeaponSO.TrailConfig.Duration);
    //    yield return null;
    //    instance.emitting = false;
    //    instance.gameObject.SetActive(false);
    //    MyWeaponSO.TrailPool.Release(instance);
    //}


    //private TrailRenderer CreateTrail()
    //{
    //    GameObject instance = new GameObject("Bullet Trail");
    //    TrailRenderer trail = instance.AddComponent<TrailRenderer>();
    //    trail.colorGradient = MyWeaponSO.TrailConfig.Color;
    //    trail.material = MyWeaponSO.TrailConfig.Material;
    //    trail.widthCurve = MyWeaponSO.TrailConfig.WidthCurve;
    //    trail.time = MyWeaponSO.TrailConfig.Duration;
    //    trail.minVertexDistance = MyWeaponSO.TrailConfig.MinVertexDistance;

    //    trail.emitting = false;
    //    trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

    //    return trail;
    //}
    #endregion
}
