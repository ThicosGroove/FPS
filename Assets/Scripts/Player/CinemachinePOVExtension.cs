using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [Header("View Movement")]
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAnlge = 80f;

    [Header("Tilt Movement")]
    [SerializeField] private int dutch = 2;
    [SerializeField] private float smooth = 1f;
    [SerializeField] private float timeToTilt = 1f;

    CinemachineVirtualCamera virtualCamera;

    private InputManager input;
    private Vector3 startingRotation;
    private Camera mainCamera;

    protected override void Awake()
    {
        input = InputManager.Instance;
        mainCamera = Camera.main;

        base.Awake();
    }

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraFollow();
        CameraTilt();
    }


    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;

                if (Application.isPlaying)
                {
                    Vector2 deltaInput = input.GetPlayerMouseMovement();

                    startingRotation.x += deltaInput.x * horizontalSpeed * Time.deltaTime;
                    startingRotation.y += deltaInput.y * verticalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAnlge, clampAnlge);
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                }
            }
        }
    }

    private void CameraFollow()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }

    private void CameraTilt()
    {
        Vector2 deltaInput = input.GetPlayerMovement();

        virtualCamera.m_Lens.Dutch = deltaInput.x > 0 ? -CalculateTilt(dutch) : deltaInput.x < 0 ? CalculateTilt(dutch) : CalculateTilt(0);
        //virtualCamera.m_Lens.Dutch = deltaInput.x > 0 ? -CalculateTilt(tilt) : CalculateTilt(tilt);
    }

    private float CalculateTilt(int tilt)
    {
        float normalDutch = virtualCamera.m_Lens.Dutch;

        //return Mathf.MoveTowards(normalDutch, tilt, (timeToTilt / smooth) * Time.deltaTime);
        return Mathf.Lerp(0, tilt, 0.5f * Time.deltaTime);
    }
}
