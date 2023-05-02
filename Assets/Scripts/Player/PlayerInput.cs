using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FPSMove)), RequireComponent(typeof(PlayerWeaponAction)), RequireComponent(typeof(PlayerWeaponSelector))]
public class PlayerInput : MonoBehaviour
{
    InputControl _input;

    [SerializeField] private bool _isShootCharging = false;
    [SerializeField] private bool _isShootStarted = false;
    [SerializeField] private bool _isShootGoOff = false;

    public bool IsShootStarted { get { return _isShootStarted; } set { _isShootStarted = value; } }
    public bool IsShootCharging { get { return _isShootCharging; } set { _isShootCharging = value; } }
    public bool IsShootGoOff { get { return _isShootGoOff; } set { _isShootGoOff = value; } }


    void Awake()
    {
        _input = new InputControl();
    }

    private void Start()
    {
        _input.Character.Shoot.started += OnShootStart;
        _input.Character.Shoot.performed += OnShootCharge;
        _input.Character.Shoot.canceled += OnShootGoOff;

        _isShootGoOff = false;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Character.Shoot.started -= OnShootStart;
        _input.Character.Shoot.performed -= OnShootCharge;
        _input.Character.Shoot.canceled -= OnShootGoOff;
    }

    public Vector2 GetPlayerMovement()
    {
        return _input.Character.Movement.ReadValue<Vector2>();
    }

    public bool IsPlayerMoving()
    {
        Vector2 newInput = _input.Character.Movement.ReadValue<Vector2>();

        if (Mathf.Abs(newInput.x) > Mathf.Epsilon || Mathf.Abs(newInput.y) > Mathf.Epsilon) return true;

        else return false;
    }

    public Vector2 GetPlayerMouseMovement()
    {
        return _input.Character.View.ReadValue<Vector2>();
    }

    public bool GetPlayerDashThisFrame()
    {
        return _input.Character.Dash.triggered;
    }

    public bool GetPlayerJumpThisFrame()
    {
        return _input.Character.Jump.triggered;
    }


    private void OnShootStart(InputAction.CallbackContext context)
    {
        _isShootStarted = true;
    }

    public void OnShootCharge(InputAction.CallbackContext context)
    {

        _isShootCharging = true;
    }

    private void OnShootGoOff(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _isShootCharging = false;
            _isShootGoOff = true;
        }
    }

    public bool PlayerChangeWeaponNext()
    {
        return _input.Character.ChangeWeaponNext.triggered;
    }
}
