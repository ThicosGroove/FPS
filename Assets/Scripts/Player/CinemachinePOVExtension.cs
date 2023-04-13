using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAnlge = 80f;

    private InputManager input;
    private Vector3 startingRotation;
    private Camera mainCamera;
    private Camera myCamera;

    protected override void Awake()
    {
        input = InputManager.Instance;
        mainCamera = Camera.main;

        base.Awake();

    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }


    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                Vector2 deltaInput = input.GetPlayerMouseMovement();
                startingRotation.x += deltaInput.x * horizontalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * verticalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAnlge, clampAnlge);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);               
            }
        }
    }
}
