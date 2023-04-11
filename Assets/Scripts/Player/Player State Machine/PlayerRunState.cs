using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory) 
    : base (currentContext, playerStateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("RUN STATE");
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
      
    }

    public override void UpdateState()
    {
        HandleRunning();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (!CTX.IsMoving)
        {
            SwitchState(Factory.Idle());
        }
        else if (CTX.IsMoving && !CTX.IsSprinting)
        {
            SwitchState(Factory.Walk());
        }
    }

    void HandleRunning()
    {
        CTX.MoveDirection = CTX.transform.TransformDirection(Vector3.right) * CTX.CurrentInput.x * CTX.SprintSpeed + CTX.transform.TransformDirection(Vector3.forward).normalized * CTX.CurrentInput.y * CTX.SprintSpeed;
        CTX.CharacterController.Move(CTX.MoveDirection * Time.deltaTime);
    }
}
