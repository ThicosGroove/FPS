using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourretEnemy : AEnemy
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void DistanceToWake(GameObject target, float distToWake)
    {

    }

    protected override void MovementTowardsPlayer(GameObject target)
    {

    }

    protected override void PatrolMovement()
    {

    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public override void LostHealth(float damageRecieved)
    {
        healthBar.gameObject.SetActive(true);
        HealthBarFiller(damageRecieved);
    }

    protected override void Die()
    {
        
    }
}
