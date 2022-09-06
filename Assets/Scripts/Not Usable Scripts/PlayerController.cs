using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSettings;

public class PlayerController : MonoBehaviour
{

    private InputControl input;
    private CharacterController characterController;
    private Rigidbody rb;

    [Header("Gravity Settings")]
    public Transform groundCheckObj;
    private Vector3 fallVelocity;
    public LayerMask groundLayerMask;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private bool isGrounded;

    [Header("Movement Settings")]
    [SerializeField] private PlayerSettingsModel playerSettings;
    [SerializeField] private Vector3 movementDirection;
    [SerializeField] private Vector2 input_Movement;
    [SerializeField, Range(6,12)] private float moveVelocity = 12;

    [Header("Jump Settings")]
    private Vector3 jumpHeight;
    [SerializeField] private bool hasJump;
    [SerializeField] private float jumpForce = 5f;

    private void Awake()
    {
        input = new InputControl();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

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
        Jump();
    }

    private void ControlMovement()
    {
        var verticalSpeed = playerSettings.WalkingFowardSpeed * input_Movement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Movement.x * Time.deltaTime;

        movementDirection = new Vector3(horizontalSpeed, 0, verticalSpeed).normalized;
        movementDirection = transform.TransformVector(movementDirection);

        characterController.Move(movementDirection * moveVelocity * Time.deltaTime);
    }

    private void Jump()
    {

    }

    private bool GroundCheck()
    {
        fallVelocity.y += gravity * Time.deltaTime;

        float groundDistance = 0.5f;
        isGrounded = Physics.CheckSphere(groundCheckObj.position, groundDistance, groundLayerMask);

        if (isGrounded && fallVelocity.y < 0)
        {
            fallVelocity.y = -2f;
        }

        characterController.Move(fallVelocity * Time.deltaTime);

        return isGrounded;
    }
}
