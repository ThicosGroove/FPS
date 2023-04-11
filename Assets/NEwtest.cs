using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEwtest : MonoBehaviour
{
    MeshRenderer rha;

    // Start is called before the first frame update
    void Start()
    {
        rha = GetComponent<MeshRenderer>();

        rha.material.color = new Color(1f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
