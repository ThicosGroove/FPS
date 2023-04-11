using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }


    public override void EnterState()
    {
        Debug.LogWarning("Hello From Grounded State");
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
        HandleGravity();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (CTX.ShouldJump)
        {
            SwitchState(Factory.Jump());
        }
    }


    void HandleGravity()
    {
        CTX.MoveDirectionY -= CTX.Gravity * CTX.GravityMultiplier * Time.deltaTime;
        CTX.CharacterController.Move(CTX.MoveDirection * Time.deltaTime);
    }

}
