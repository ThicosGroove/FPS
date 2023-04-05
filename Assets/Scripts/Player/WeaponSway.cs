using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMulti;

    InputManager input;

    private void Start()
    {
        input = InputManager.Instance;
    }

    void Update()
    {
        float mouseX = input.GetPlayerMouseMovement().x;
        float mouseY = input.GetPlayerMouseMovement().y;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion tartegRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, tartegRotation, smooth * Time.deltaTime);
    }
}
