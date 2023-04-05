using UnityEngine;

public abstract class PlayerBaseState
{
   public abstract void EnterState(PlayerStateManager ctx);

   public abstract void UpdateState(PlayerStateManager ctx);

   public abstract void ExitState(PlayerStateManager ctx);
}
