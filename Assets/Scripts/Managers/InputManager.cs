using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private bool _isShootCharging = false;
    private bool _isShootStarted = false;
    private bool _isShootGoOff = false;

    public bool IsShootStarted { get { return _isShootStarted; } set { _isShootStarted = value; } }
    public bool IsShootCharging { get { return _isShootCharging; } set { _isShootCharging = value; } }
    public bool IsShootGoOff { get { return _isShootGoOff; } set { _isShootGoOff = value; } }

    InputControl _input;

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
        _input = new InputControl();
    }

    private void Start()
    {
        _input.Character.Shoot.started += OnShootStart;
        _input.Character.Shoot.performed += OnShootCharge;
        _input.Character.Shoot.canceled += OnShootGoOff;
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

    #region Player Input
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

    public bool GetPlayerCrouch()
    {
        return _input.Character.Crouch.triggered;
    }

    public bool GetPlayerAim()
    {
        return _input.Character.Aim.triggered;
    }

    private void OnShootStart(InputAction.CallbackContext context)
    {
        _isShootStarted = true;
        Debug.Log("Shoot Start Callback called");
    }

    public void OnShootCharge(InputAction.CallbackContext context)
    {
        _isShootCharging = true;
        Debug.Log("Shoot Performed Callback called");
    }

    private void OnShootGoOff(InputAction.CallbackContext context)
    {
        Debug.Log("Insinde context.canceled");
        _isShootCharging = false;
        _isShootGoOff = true;
 
    }

    public bool PlayerChangeWeaponNext()
    {
        return _input.Character.ChangeWeaponNext.triggered;
    }


    #endregion

    #region System Input

    public bool PlayerPause()
    {
        return _input.System.Pause.triggered;
    }

    #endregion

}
