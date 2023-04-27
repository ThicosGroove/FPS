using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMulti;

    InputManager _input;

    private void Start()
    {
        _input = InputManager.Instance;
    }

    void Update()
    {
        var mouseX = _input.GetPlayerMouseMovement().x;
        var mouseY = _input.GetPlayerMouseMovement().y;

        var rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        var rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        var tartegRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, tartegRotation, smooth * Time.deltaTime);
    }
}
