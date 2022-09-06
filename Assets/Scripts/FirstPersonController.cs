using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

    public bool CandMove { get; private set; } = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3f;
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
        if (CandMove)
        {
            HandleMovementInput();

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x + (transform.TransformDirection(Vector3.right) * currentInput.y));
        moveDirection.y = moveDirectionY;

    }


    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

}
