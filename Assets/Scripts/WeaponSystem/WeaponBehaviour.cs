using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(WeaponShootBehaviour))]
public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] public WeaponSO MyWeaponSO;

    [Space]
    [SerializeField] private ParticleSystem _particleCharging;

    private WeaponShootBehaviour _weaponShootBehaviour;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        _weaponShootBehaviour = GetComponent<WeaponShootBehaviour>();

        MyWeaponSO.ActiveMonoBehaviour = this;

        if (!_anim)
        {
            Debug.LogError("Não achou animator");
        }

    }

    // Animação para cada tiro do combo

    public void StartChargingParticles()
    {
        if (_particleCharging.isPlaying) return;
        Debug.LogWarning("START CHARGING");

        _particleCharging.Play();
    }

    public void StopChargingParticles()
    {
        if (!_particleCharging.isPlaying) return;
        Debug.LogWarning("STOP CHARGING");

        _particleCharging.Stop();
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

    
}
