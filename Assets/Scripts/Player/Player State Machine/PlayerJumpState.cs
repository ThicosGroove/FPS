using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.LogWarning("Hello From Jump State");

        HandleJump();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        if (!CTX.IsMoving)
        {
            SetSubState(Factory.Idle());
        }
        else if (!CTX.IsSprinting && !CTX.ShouldCrouch && !CTX.IsAiming)
        {
            Debug.LogWarning("WALK SUB STATE");
            SetSubState(Factory.Walk());
        }
        else if (CTX.IsSprinting)
        {
            SetSubState(Factory.Run());
        }
        else if (CTX.ShouldCrouch && !CTX.IsSprinting)
        {
            SetSubState(Factory.Crouch());
        }
    }

    public override void UpdateState()
    {
        InitializeSubState();
        HandleJump();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (CTX.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    void HandleJump()
    {
        HandleGravity();

        if (CTX.CharacterController.isGrounded)
        {
            CTX.MoveDirectionY = CTX.JumpForce;
        }
    }

    void HandleGravity()
    {
        CTX.MoveDirectionY -= CTX.Gravity * Time.deltaTime;
        CTX.CharacterController.Move(CTX.MoveDirection * Time.deltaTime);
    }
}
