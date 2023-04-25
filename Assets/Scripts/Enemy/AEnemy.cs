using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask plataformLayerMask;

    [SerializeField] protected float maxHealth;
    protected float patrolSpeed { get; set; }
    protected float chaseSpeed { get; set; }


    [SerializeField] protected GameObject healthBar;
    [SerializeField] private Image filledHealthtBar;
    //private Animator anim;

    Rigidbody2D rb;

    private float currentHealth;
    private float filledSpeed = 2f;

    protected State state;
    protected enum State
    {
        Sleep,
        Patrol,
        Chase
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        healthBar.gameObject.SetActive(false);
        currentHealth = maxHealth;
    }

    protected abstract void MovementTowardsPlayer(GameObject target);

    protected abstract void PatrolMovement();

    protected abstract void DistanceToWake(GameObject target, float distToWake);

    protected abstract void OnCollisionEnter2D(Collision2D collision);

    public abstract void LostHealth(float damageRecieved);

    protected abstract void Die();

    protected virtual void HealthBarFiller(float damage)
    {
        currentHealth -= damage; 
        float fillAmountPercentage = currentHealth / maxHealth;

        filledHealthtBar.fillAmount = Mathf.Lerp(filledHealthtBar.fillAmount, fillAmountPercentage, 1);
    }

    //IEnumerator PlayHitAnimation()
    //{
    //    anim.SetBool("isHit", true);

    //    yield return new WaitForSeconds(0.15f);

    //    anim.SetBool("isHit", false);
    //}

    // For main Body, Hit by Player and Arrow 
 

    // Both Triggers are For foot, used to flip
    protected void OnTriggerExit2D(Collider2D collision)
    {
 
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
     
    }

}