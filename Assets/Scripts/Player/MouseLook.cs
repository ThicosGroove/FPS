using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSettings;

public class MouseLook : CinemachineExtension
{
    [Header("References")]
    public Transform playerBody;

    [Header("Rotation Settings")]
    [SerializeField] private Vector2 input_View;
    [SerializeField] private PlayerSettingsModel playerSettings;

    private Vector3 startingRotation;

    private InputManager input;

    private float RotationX = 0f;

    private void Start()
    {
        playerSettings = new PlayerSettingsModel();
        Cursor.lockState = CursorLockMode.Locked;
        input = InputManager.Instance;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                Vector2 deltaInput = input.GetPlayerMouseMovement();
                startingRotation.x += deltaInput.x * playerSettings.ViewSensitivityX * ViewControl().x * Time.deltaTime;
                startingRotation.y += deltaInput.y * playerSettings.ViewSensitivityY * ViewControl().y * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -playerSettings.clampAngle, playerSettings.clampAngle);

                state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0);
            }
        }

    }

    Vector2 ViewControl()
    {
        input_View.x = playerSettings.ViewIntertedX ? -input_View.x : input_View.x;
        input_View.y = playerSettings.ViewIntertedY ? input_View.y : -input_View.y;
        return input_View;
    }
}
