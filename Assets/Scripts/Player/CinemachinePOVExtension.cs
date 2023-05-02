using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CinemachinePOVExtension : CinemachineExtension
{
    [Header("View Movement")]
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAnlge = 80f;

    [Header("Tilt Movement")]
    [SerializeField] private int tilt = 2;
    [SerializeField] private float smooth = 1f;
    [SerializeField] private float timeToTilt = 1f;

    private CinemachineVirtualCamera _virtualCamera;
    private Camera _mainCamera;

    private PlayerInput _input;
    private Vector3 _startingRotation;

    protected override void Awake()
    {
        _input = GetComponentInParent<PlayerInput>();
        _mainCamera = Camera.main;

        base.Awake();
    }

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
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
                if (_startingRotation == null) _startingRotation = transform.localRotation.eulerAngles;

                if (Application.isPlaying)
                {
                    Vector2 deltaInput = _input.GetPlayerMouseMovement();

                    _startingRotation.x += deltaInput.x * horizontalSpeed * Time.deltaTime;
                    _startingRotation.y += deltaInput.y * verticalSpeed * Time.deltaTime;
                    _startingRotation.y = Mathf.Clamp(_startingRotation.y, -clampAnlge, clampAnlge);
                    state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
                }
            }
        }
    }

    private void CameraFollow()
    {
        transform.position = _mainCamera.transform.position;
        transform.rotation = _mainCamera.transform.rotation;
    }

    // Usar o DOTween para rotacionar a camera de forma mais controlada
    private void CameraTilt()
    {
        Vector2 deltaInput = _input.GetPlayerMovement();

        //if (deltaInput.x > 0)
        //{
        //    CalculateTilt(tilt);
        //}
        //else if (deltaInput.x < 0)
        //{
        //    CalculateTilt(-tilt);
        //}
        //else
        //{
        //    CalculateTilt(0);
        //}

        //tilt = deltaInput.x > 0 ? CalculateTilt(tilt) : deltaInput.x < 0

        //_virtualCamera.m_Lens.Dutch = deltaInput.x > 0 ? CalculateTilt(dutch) : deltaInput.x < 0 ? CalculateTilt(dutch) : CalculateTilt(0);
        //virtualCamera.m_Lens.Dutch = deltaInput.x > 0 ? -CalculateTilt(tilt) : CalculateTilt(tilt);
    }

    // Usar DoTween para controlar o FOV de forma mais controlada
    private void CameraFOV()
    {
    }

    //private void CalculateTilt(int tilt)
    //{
    //    var tiltAngle = new Vector3(0f, 0f, tilt);
    //    transform.DORotate(tiltAngle, timeToTilt, RotateMode.Fast);


    //    return Mathf.MoveTowards(normalDutch, tilt, (timeToTilt / smooth) * Time.deltaTime);

    //}

}
