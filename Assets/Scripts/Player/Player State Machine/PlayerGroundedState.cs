using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

 

    public override void EnterState()
    {
        
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

    public override void CheckSwitchStates()
    {
        if (_ctx.ShouldJump)
        {
            SwitchState(_factory.Jump());
        }
    }
}
