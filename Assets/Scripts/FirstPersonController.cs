using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => characterController.isGrounded && Input.GetKeyDown(jumpKey);

    [Header("Fucnitonal Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 30f;

    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();

            if (canJump)
                HanldeJump();

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2((IsSprinting? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward)) * currentInput.x + (transform.TransformDirection(Vector3.right)) * currentInput.y;
        moveDirection.y = moveDirectionY;

    }


    private void HanldeJump()
    {
        if (ShouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;



        characterController.Move(moveDirection * Time.deltaTime);
    }

}
