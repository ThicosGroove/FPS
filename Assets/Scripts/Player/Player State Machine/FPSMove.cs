using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask groundMask;

    [Header("Player Movement")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 15f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpGravity = -9.81f;
    [SerializeField] private float groundedGravity = 0;

    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTimer = 0.8f;

    private InputManager _input;
    private CharacterController _controller;
    private Transform _camTransform;

    private bool _ShouldDash => _input.GetPlayerDashThisFrame();
    private bool _ShouldJump => _input.GetPlayerJumpThisFrame();
    private bool _ShouldCrouch => _input.GetPlayerCrouch();
    private bool _isAiming => _input.GetPlayerAim();

    private Vector3 _playerVelocity;
    private Vector3 _moveDir;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _camTransform = Camera.main.transform;
        _input = InputManager.Instance;
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
        var movement = _input.GetPlayerMovement();
        _moveDir = new Vector3(movement.x, 0, movement.y).normalized;
        _moveDir = _camTransform.forward * _moveDir.z + _camTransform.right * _moveDir.x;

        _controller.Move(_moveDir * Time.deltaTime * playerSpeed);
    }

    private void HandleJump()
    {
        if (_ShouldJump && groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * jumpGravity);
        }
    }

    private bool ChechingGround()
    {
        var ray = Physics.CheckSphere(groundCheckPos.position, .2f, groundMask);

        if (ray) return true;
        else return false;
    }

    private void HandleGravity()
    {
        if (groundedPlayer && _playerVelocity.y < 0)
            _playerVelocity.y = groundedGravity;

        _playerVelocity.y += jumpGravity * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void HandleDash()
    {
        if (_ShouldDash)
        {
            StartCoroutine(DashCO());
        }
    }

    IEnumerator DashCO()
    {
        var startTime = Time.time;

        while (Time.time < startTime + dashTimer)
        {
            _controller.Move(_moveDir * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
}