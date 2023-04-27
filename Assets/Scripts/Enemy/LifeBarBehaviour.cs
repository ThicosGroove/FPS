using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarBehaviour : MonoBehaviour
{
    Transform _camTransform;

    void Start()
    {
        _camTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(transform.position + _camTransform.rotation * Vector3.forward, _camTransform.rotation * Vector3.up);
    }
}
