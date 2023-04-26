using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private bool isShootCharging = false;
    private bool isShootStarted = false;
    private bool isShootGoOff = false;

    public bool IsShootStarted { get { return isShootStarted; } set { isShootStarted = value; } }
    public bool IsShootCharging { get { return isShootCharging; } set { isShootCharging = value; } }
    public bool IsShootGoOff { get { return isShootGoOff; } set { isShootGoOff = value; } }

    InputControl input;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //base.Awake();
        input = new InputControl();
    }

    private void Start()
    {
        input.Character.Shoot.started += OnShootStart;
        input.Character.Shoot.performed += OnShootCharge;
        input.Character.Shoot.canceled += OnShootGoOff;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();

        input.Character.Shoot.started -= OnShootStart;
        input.Character.Shoot.performed -= OnShootCharge;
        input.Character.Shoot.canceled -= OnShootGoOff;
    }

    #region Player Input
    public Vector2 GetPlayerMovement()
    {
        return input.Character.Movement.ReadValue<Vector2>();
    }

    public bool IsPlayerMoving()
    {
        Vector2 newInput = input.Character.Movement.ReadValue<Vector2>();

        if (Mathf.Abs(newInput.x) > Mathf.Epsilon || Mathf.Abs(newInput.y) > Mathf.Epsilon) return true;

        else return false;
    }

    public Vector2 GetPlayerMouseMovement()
    {
        return input.Character.View.ReadValue<Vector2>();
    }

    public bool GetPlayerDashThisFrame()
    {
        return input.Character.Dash.triggered;
    }

    public bool GetPlayerJumpThisFrame()
    {
        return input.Character.Jump.triggered;
    }

    public bool GetPlayerCrouch()
    {
        return input.Character.Crouch.triggered;
    }

    public bool GetPlayerAim()
    {
        return input.Character.Aim.triggered;
    }

    private void OnShootStart(InputAction.CallbackContext context)
    {
        isShootStarted = true;
    }

    public void OnShootCharge(InputAction.CallbackContext context)
    {

        isShootCharging = true;
    }

    private void OnShootGoOff(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            IsShootCharging = false;
            isShootGoOff = true;
        }
    }

    public bool PlayerChangeWeaponNext()
    {
        return input.Character.ChangeWeaponNext.triggered;
    }


    #endregion

    #region System Input

    public bool PlayerPause()
    {
        return input.System.Pause.triggered;
    }

    #endregion

}
