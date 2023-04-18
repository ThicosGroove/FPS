using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    private InputManager input;
    private CharacterController controller;
    private Transform camTransform;

    private bool ShouldDash => input.GetPlayerDashThisFrame();
    private bool ShouldJump => input.GetPlayerJumpThisFrame();
    private bool ShouldCrouch => input.GetPlayerCrouch();
    private bool isAiming => input.GetPlayerAim();

    private Vector3 playerVelocity;
    private Vector3 moveDir;


    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos1;
    [SerializeField] private Transform groundCheckPos2;
    [SerializeField] private LayerMask groundMask;

    [Header("Player Movement")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 15f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTimer = 0.8f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
        input = InputManager.Instance;
    }

    void Update()
    {
        HandleGravity();

        groundedPlayer = ChechingGround();

        HandleMovement();

        HandleJump();

        HandleDash();
    }

    private void HandleMovement()
    {
        Vector2 movement = input.GetPlayerMovement();
        moveDir = new Vector3(movement.x, 0, movement.y).normalized;
        moveDir = camTransform.forward * moveDir.z + camTransform.right * moveDir.x;

        controller.Move(moveDir * Time.deltaTime * playerSpeed);
    }

    private void HandleJump()
    {
        if (input.GetPlayerJumpThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private bool ChechingGround()
    {
        bool ray1 = Physics.CheckSphere(groundCheckPos1.position, .1f, groundMask);
        bool ray2 = Physics.CheckSphere(groundCheckPos2.position, .1f, groundMask);

        if (ray1 || ray2)
            return true;
        else return false;
    }

    private void HandleGravity()
    {
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleDash()
    {
        if (input.GetPlayerDashThisFrame())
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTimer)
        {
            controller.Move(moveDir * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
}