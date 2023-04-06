using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void CheckSwitchStates()
    {
        
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void ExitState()
    {
    
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {

    }

    void HandleJump()
    {
        _ctx.MoveDirectionY = _ctx.JumpForce;
    }
}
