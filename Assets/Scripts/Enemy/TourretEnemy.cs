using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourretEnemy : AEnemy
{
    private bool _isPlayerInRange = false;
    private GameObject _currentPlayer;

    void Update()
    {
        switch (_state)
        {
            case State.Sleep:
               
                break;
            case State.Patrol:
                
                break;
            case State.Chase:
                MovementTowardsPlayer();
                AttackBehaviour();
                break;
            default:
                break;
        }
    }

    public override void TriggerEnter(GameObject player)
    {
        _isPlayerInRange = true;
        _currentPlayer = player;
        _state = State.Chase;
    }

    public override void TriggerStay()
    {
        
    }

    public override void TriggerExit()
    {
        _isPlayerInRange = false;
        _state = State.Sleep;
    }

    protected override void MovementTowardsPlayer()
    {
        transform.LookAt(_currentPlayer.transform);
    }

    protected override void PatrolMovement()
    {

    }

   
    protected override void AttackBehaviour()
    {
        // Attack 
    }

    public override void LostHealth(float damageRecieved)
    {
        healthBar.gameObject.SetActive(true);
        HealthBarFiller(damageRecieved);

        TextDamage indicator = Instantiate(damageText, damageTextPos.position, Quaternion.identity).GetComponent<TextDamage>();
        indicator.SetDamageText((int)damageRecieved);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Destroy(this.gameObject.GetComponentInParent<Transform>().gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        var damage = other.GetComponent<ParticleBehaviour>().finalDamage;

        Debug.LogWarning($"Acertou {damage}");

        LostHealth(damage);
    }
}
