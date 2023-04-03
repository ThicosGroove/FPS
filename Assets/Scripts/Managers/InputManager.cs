using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    InputControl input;

    protected override void Awake()
    {
        base.Awake();
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

    public Vector2 GetPlayerMovement()
    {
        return input.Character.Movement.ReadValue<Vector2>();
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

}
