using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSettings;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 input_Movement;
    [SerializeField] private Vector2 input_View;
    [SerializeField] private Vector3 newCameraRotation;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    [SerializeField] private float minCameraRotationX;
    [SerializeField] private float maxCameraRotationX;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;

    private InputControl input;
    private CharacterController characterController;

    private void Awake()
    {
        input = new InputControl();
        characterController = GetComponent<CharacterController>();

        input.Character.Movement.performed += ctx => input_Movement = ctx.ReadValue<Vector2>();
        input.Character.View.performed += ctx => input_View = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        newCameraRotation = cameraHolder.localRotation.eulerAngles;
    }

    private void Update()
    {
        ControlView();
        ControlMovement();
    }

    private void ControlView()
    {
        input_View.y = playerSettings.ViewIntertedY ? input_View.y: -input_View.y;
        input_View.x = playerSettings.ViewIntertedX ? -input_View.x: input_View.x;

        newCameraRotation.x += playerSettings.ViewSensitivityY * input_View.y * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, minCameraRotationX, maxCameraRotationX);

        newCameraRotation.y += playerSettings.ViewSensitivityX * input_View.x * Time.deltaTime;

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void ControlMovement()
    {
        var verticalSpeed = playerSettings.WalkingFowardSpeed * input_Movement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Movement.x * Time.deltaTime;

        var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);

        newMovementSpeed = transform.TransformDirection(newMovementSpeed);

        characterController.Move(newMovementSpeed);
    }
}
