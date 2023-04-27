using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    AEnemy _myEnemy;

    private void Start()
    {
        _myEnemy = GetComponentInParent<AEnemy>();       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.transform.gameObject;

            _myEnemy.TriggerEnter(player);
        }
    }

    private void OnTriggerStay(Collider other) => _myEnemy.TriggerStay(); 

    private void OnTriggerExit(Collider other) => _myEnemy.TriggerExit(); 
}
