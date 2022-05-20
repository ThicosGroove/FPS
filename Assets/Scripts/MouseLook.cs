using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSettings;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;

    [Header("Rotation Settings")]
    [SerializeField] private Vector2 input_View;
    [SerializeField] private PlayerSettingsModel playerSettings;

    private InputControl input;

    private float RotationX = 0f;

    private void Awake()
    {
        input = new InputControl();
        input.Character.View.performed += ctx => input_View = ctx.ReadValue<Vector2>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        ViewControl();
    }

    void ViewControl()
    {
        input_View.x = playerSettings.ViewIntertedX ? -input_View.x : input_View.x;
        input_View.y = playerSettings.ViewIntertedY ? input_View.y : -input_View.y;

        float mouseX = input_View.x * playerSettings.ViewSensitivityX * Time.deltaTime;
        float mouseY = input_View.y * playerSettings.ViewSensitivityY * Time.deltaTime;

        RotationX += mouseY;
        RotationX = Mathf.Clamp(RotationX, playerSettings.minCameraRotationX, playerSettings.maxCameraRotationX);

        transform.localRotation = Quaternion.Euler(RotationX, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
