using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }



    public override void EnterState()
    {
        Debug.LogWarning("iDLE STATE");
        CTX.MoveDirectionX = 0;
        CTX.MoveDirectionY = 0;
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
       
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (CTX.IsMoving && CTX.IsSprinting)
        {
            SwitchState(Factory.Run());
        }
        else if (CTX.IsMoving)
        {
            SwitchState(Factory.Walk());
        }
    }

}
