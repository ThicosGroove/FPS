using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    AEnemy myEnemy;

    private void Start()
    {
        myEnemy = GetComponentInParent<AEnemy>();       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.transform.gameObject;

            myEnemy.TriggerEnter(player);
        }
    }

    private void OnTriggerStay(Collider other)
    {
            myEnemy.TriggerStay();      
    }

    private void OnTriggerExit(Collider other)
    {
            myEnemy.TriggerExit();        
    }
}
