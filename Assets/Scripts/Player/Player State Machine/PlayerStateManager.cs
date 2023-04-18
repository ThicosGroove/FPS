using System.Collections;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private CharacterController _characterController;
    private Camera playerCamera;

    [Header("Fucnitonal Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadBob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;

    [Header("Controls")]
    private InputManager input;

    [Header("Movement Parameters")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _crouchSpeed = 2.5f;
    [SerializeField] private float _slopeSpeed = 8f;

    [Header("Jumping Parameters")]
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _jumpGravity = 20f;
    [SerializeField] private float _gravityMiltiplier = 150f;

    [Header("Crouch Parameters")]
    [SerializeField] private float _crouchHeight = 0.5f;
    [SerializeField] private float _standingHeight = 2f;
    [SerializeField] private float _timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = Vector3.zero;
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("HeadBob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;

    // SLIDING PARAMETERS
    private Vector3 hitPointNormal;

    private Vector3 _moveDirection;
    //private Vector2 _currentInput;

    //States variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //private bool _isSprinting => input.GetPlayerSprint() && canSprint;
    //private bool _shouldJump => input.GetPlayerJumpThisFrame() && _characterController.isGrounded;
    //private bool _shouldCrouch => input.GetPlayerCrouch() && _characterController.isGrounded && !duringCrouchAnimation;
    //private bool _isAiming => input.GetPlayerAim();

    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isSprinting;
    [SerializeField] private bool _shouldJump;
    [SerializeField] private bool _shouldCrouch;
    [SerializeField] private bool _isAiming;
    [SerializeField] private Vector2 _currentInput;

    //Getters and Setters
    public CharacterController CharacterController { get { return _characterController; } }

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool CanMove { get; private set; } = true;

    public bool IsMoving { get { return _isMoving; } }
    public bool IsSprinting { get { return _isSprinting; } }
    public bool ShouldJump { get { return _shouldJump; } }
    public bool ShouldCrouch { get { return _shouldCrouch; } }
    public bool IsAiming { get { return _isAiming; } }
    public Vector2 CurrentInput { get { return _currentInput; } }

    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public float MoveDirectionX { get { return _moveDirection.x; } set { _moveDirection.x = value; } }
    public float MoveDirectionY { get { return _moveDirection.y; } set { _moveDirection.y = value; } }
    public float MoveDirectionZ { get { return _moveDirection.z; } set { _moveDirection.z = value; } }

    public float WalkSpeed { get { return _walkSpeed; } }
    public float SprintSpeed { get { return _sprintSpeed; } }
    public float CrouchSpeed { get { return _crouchSpeed; } }
    public float SlopeSpeed { get { return _slopeSpeed; } }

    public bool IsSliding
    {
        get
        {
            if (_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 3f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(Vector3.up, hitPointNormal) > _characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }

    }
    public float JumpForce { get { return _jumpForce; } }
    public float Gravity { get { return _jumpGravity; } }
    public float GravityMultiplier { get { return _gravityMiltiplier; } }
    public bool IsGrounded { get { return _characterController.isGrounded; } }


    private void Awake()
    {
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        _characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        defaultYPos = playerCamera.transform.localPosition.y;

    }

    void Start()
    {
        input = InputManager.Instance;
    }

    void Update()
    {
        //GetPlayerInputs();
        _currentState.UpdateStates();
        _currentInput = input.GetPlayerMovement().normalized;
        _shouldJump = input.GetPlayerJumpThisFrame();
        _isMoving = input.IsPlayerMoving();
        _isSprinting = input.GetPlayerDashThisFrame();
        _shouldCrouch = input.GetPlayerCrouch();
        _isAiming = input.GetPlayerAim();
    }

    private void GetPlayerInputs()
    {
    }

    void HandleGravity()
    {

        _moveDirection.y -= _jumpGravity * Time.deltaTime;

        //MoveDirectionY -= Gravity * Time.deltaTime;
        //_moveDirection.y = MoveDirectionY;
        CharacterController.Move(_moveDirection * Time.deltaTime);
    }

    private void HandleMovementInput()
    {
        Vector3 dir = input.GetPlayerMovement().normalized;
        _currentInput = new Vector2((isCrouching ? _crouchSpeed : _isSprinting ? _sprintSpeed : _walkSpeed) * dir.x, (isCrouching ? _crouchSpeed : _isSprinting ? _sprintSpeed : _walkSpeed) * dir.y);

        float moveDirectionY = _moveDirection.y;
        _moveDirection = transform.TransformDirection(Vector3.right) * _currentInput.x + transform.TransformDirection(Vector3.forward).normalized * _currentInput.y;
        _characterController.Move(_moveDirection * Time.deltaTime);

        _moveDirection.y = moveDirectionY;
    }

    private void HanldeJump()
    {
        if (_shouldJump)
            _moveDirection.y = _jumpForce;
    }

    private void HandleCrouch()
    {
        if (_shouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private void HandleHeadBob()
    {
        if (!_characterController.isGrounded) return;

        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : _isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : _isSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void ApplyFinalMovements()
    {
        if (!_characterController.isGrounded)
            _moveDirection.y -= _jumpGravity * Time.deltaTime;

        if (WillSlideOnSlopes && IsSliding)
            _moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * _slopeSpeed;


        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1.5f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? _standingHeight : _crouchHeight;
        float currentHeight = _characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = _characterController.center;

        while (timeElapsed < _timeToCrouch)
        {
            _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _timeToCrouch);
            _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _characterController.height = targetHeight;
        _characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }
}
