using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSettings;

public class PlayerController : MonoBehaviour
{

    private InputControl input;
    private CharacterController characterController;

    [Header("Gravity Settings")]   
    [SerializeField] private float gravity = -20f;
    public Transform groundCheckObj;
    private Vector3 velocity;
    public LayerMask groundLayerMask;
    private bool isGrounded;

    [Header("Movement Settings")]
    [SerializeField] private PlayerSettingsModel playerSettings;
    [SerializeField] private Vector2 input_Movement;

    private void Awake()
    {
        input = new InputControl();
        characterController = GetComponent<CharacterController>();

        input.Character.Movement.performed += ctx => input_Movement = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }


    private void Update()
    {
        ControlMovement();
        GroundCheck();
    }

    private void ControlMovement()
    {
        var verticalSpeed = playerSettings.WalkingFowardSpeed * input_Movement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Movement.x * Time.deltaTime;

        var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        newMovementSpeed = transform.TransformVector(newMovementSpeed);

        characterController.Move(newMovementSpeed);
    }

    private void GroundCheck()
    {
        velocity.y += gravity * Time.deltaTime;

        float groundDistance = 0.5f;
        isGrounded = Physics.CheckSphere(groundCheckObj.position, groundDistance, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}
