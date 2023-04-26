using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField] protected GameObject damageText;
    [SerializeField] protected Transform damageTextPos;

    [Space]
    [SerializeField] protected float maxHealth;
    protected float patrolSpeed { get; set; }
    protected float chaseSpeed { get; set; }

    [Space]
    [SerializeField] protected GameObject healthBar;
    [SerializeField] private Image filledHealthtBar;
    //private Animator anim;

    Rigidbody2D rb;

    protected float currentHealth;
    //private float filledSpeed = 2f;

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

        state = State.Sleep;
        currentHealth = maxHealth;
    }

    protected abstract void MovementTowardsPlayer();

    protected abstract void PatrolMovement();
    protected abstract void AttackBehaviour();

   // protected abstract void DistanceToWake();

    public abstract void LostHealth(float damageRecieved);

    protected abstract void Die();

    protected virtual void HealthBarFiller(float damage)
    {
        currentHealth -= damage; 
        float fillAmountPercentage = currentHealth / maxHealth;

        filledHealthtBar.fillAmount = Mathf.Lerp(filledHealthtBar.fillAmount, fillAmountPercentage, 1);
    }

    public abstract void TriggerEnter(GameObject player);
    public abstract void TriggerStay();
    public abstract void TriggerExit();

    //IEnumerator PlayHitAnimation()
    //{
    //    anim.SetBool("isHit", true);

    //    yield return new WaitForSeconds(0.15f);

    //    anim.SetBool("isHit", false);
    //}

    // For main Body, Hit by Player and Arrow 


    // Both Triggers are For foot, used to flip
}