public class PlayerStateFactory
{
    PlayerStateManager _context;

    public PlayerStateFactory(PlayerStateManager currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    } 
    
    public PlayerBaseState Run()
    {
        return new PlayerRunState(_context, this);
    } 
    
    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, this);
    } 

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
    
    public PlayerBaseState Crouch()
    {
        return new PlayerCrouchState(_context, this);
    } 
    
    public PlayerBaseState Aim()
    {
        return new PlayerAimState(_context, this);
    }
}
