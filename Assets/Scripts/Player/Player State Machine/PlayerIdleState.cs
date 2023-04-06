using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void CheckSwitchStates()
    {
     
    }

    public override void EnterState()
    {
        Debug.LogWarning("Hello from the Idle State");
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

}
