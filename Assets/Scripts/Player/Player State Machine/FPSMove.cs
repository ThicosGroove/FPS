using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool groundedPlayer;

    [Header("Player Movement")]
    [SerializeField] private float playerSpeed = 15f;

    [Header("Jump Paramenters")]
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpGravity = -9.81f;
    [SerializeField] private float groundedGravity = 0;
    [SerializeField] private float delayForSecondJumpSeconds = 0.5f;
    [SerializeField] private bool canSecondJump = false;

    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTimer = 0.8f;

    private PlayerInput _input;
    private CharacterController _controller;
    private Transform _camTransform;

    private bool _ShouldDash => _input.GetPlayerDashThisFrame();
    private bool _ShouldJump => _input.GetPlayerJumpThisFrame();
    //private bool _ShouldCrouch => _input.GetPlayerCrouch();
    //private bool _isAiming => _input.GetPlayerAim();

    private Vector3 _playerVelocity;
    private Vector3 _moveDir;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _camTransform = Camera.main.transform;
        _input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        HandleGravity();

        groundedPlayer = ChechingGround();

        HandleMovement();

        HandleJump();
        HandleSecondJump();

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

            StartCoroutine(DelayForSecondJumpCO());
        }
    }

    private IEnumerator DelayForSecondJumpCO()
    {
        yield return new WaitForSeconds(delayForSecondJumpSeconds);
        canSecondJump = true;
    } 

    private void HandleSecondJump()
    {
        if (_ShouldJump && canSecondJump)
        {
            _playerVelocity.y = 0;
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * jumpGravity);
            canSecondJump = false;
        }
    }

    private bool ChechingGround()
    {
        var ray = Physics.CheckSphere(groundCheckPos.position, .2f, groundMask);

        if (ray)
        {
            canSecondJump = false;
            return true;
        }
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

    private IEnumerator DashCO()
    {
        var startTime = Time.time;

        while (Time.time < startTime + dashTimer)
        {
            _controller.Move(_moveDir * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
}