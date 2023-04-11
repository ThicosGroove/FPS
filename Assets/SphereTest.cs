using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{
    MeshRenderer mesh;

    Rigidbody rb;
    [SerializeField] float velocidade = 5f; 
        

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        mesh.material.color = new Color(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector3(1, 1, 0) * velocidade, ForceMode.Force);
    }
}
