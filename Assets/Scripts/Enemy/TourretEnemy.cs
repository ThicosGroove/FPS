using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourretEnemy : AEnemy
{
    private bool IsPlayerInRange = false;
    private GameObject currentPlayer;

    void Update()
    {
        switch (state)
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

        LookingForPlayer();
    }

    private void LookingForPlayer()
    {
      
    }



    void OnDrawGizmos()
    {

    }

    protected override void MovementTowardsPlayer()
    {
        transform.LookAt(currentPlayer.transform);
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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Destroy(this.gameObject.GetComponentInParent<Transform>().gameObject);
    }

    public override void TriggerEnter(GameObject player)
    {
        IsPlayerInRange = true;
        currentPlayer = player;
        state = State.Chase;
    }

    public override void TriggerStay()
    {
        
    }

    public override void TriggerExit()
    {
        IsPlayerInRange = false;
        state = State.Sleep;
    }

}
