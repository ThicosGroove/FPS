using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }

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


        input = new InputControl();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    #region Player Input
    public Vector2 GetPlayerMovement()
    {
        return input.Character.Movement.ReadValue<Vector2>();
    }

    public bool IsPlayerMoving()
    {
        Vector2 newInput = input.Character.Movement.ReadValue<Vector2>();

        if (Mathf.Abs(newInput.x) > Mathf.Epsilon || Mathf.Abs( newInput.y) > Mathf.Epsilon) return true;

        else return false;
    }

    public Vector2 GetPlayerMouseMovement()
    {
        return input.Character.View.ReadValue<Vector2>();
    }

    public bool GetPlayerSprint()
    {
        float sprint = input.Character.Sprint.ReadValue<float>();
        return sprint > 0 ? true : false;
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

    public bool PlayerShootThisFrame()
    {
        float shoot = input.Character.Shoot.ReadValue<float>();
        return shoot > 0 ? true : false;
    }

    public bool PlayerChangeWeapon()
    {
        return input.Character.ChangeWeapon.triggered;
    }

    #endregion

    #region System Input

    public bool PlayerPause()
    {
        return input.System.Pause.triggered;
    }

    #endregion

}
