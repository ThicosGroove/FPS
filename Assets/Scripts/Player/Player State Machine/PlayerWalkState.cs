using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{

    public PlayerWalkState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    

    public override void EnterState()
    {
        Debug.LogWarning("WALK STATE"); 
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
     
    }

    public override void UpdateState()
    {
        HandleMovement();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (!CTX.IsMoving)
        {
            SwitchState(Factory.Idle());
        }
        else if (CTX.IsMoving && CTX.IsSprinting)
        {
            SwitchState(Factory.Run());
        }
    }

    void HandleMovement()
    {
        CTX.MoveDirection = CTX.transform.TransformDirection(Vector3.right) * CTX.CurrentInput.x * CTX.WalkSpeed + CTX.transform.TransformDirection(Vector3.forward).normalized * CTX.CurrentInput.y * CTX.WalkSpeed;
        CTX.CharacterController.Move(CTX.MoveDirection * Time.deltaTime);
    }


}
