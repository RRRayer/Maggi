using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    [SerializeField] private TransformAnchor _playerTransformAnchor = default;
    [SerializeField] private TransformAnchor _cameraTransformAnchor = default;
    [SerializeField] private CameraSO _currentCamera;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onSwitchCamera = default;
    [SerializeField] private VoidEventChannelSO _cameraShakeEvent = default;

    [Header("For Debug")]
    public int cameraIndex = 0;

    private CinemachineVirtualCamera[] virtualCams;

    private void OnEnable()
    {
        _inputReader.AimEvent += Aim;
        _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;
        _onSwitchCamera.OnEventRaised += SwitchToCamera;
        _cameraShakeEvent.OnEventRaised += _impulseSource.GenerateImpulse;

        _cameraTransformAnchor.Provide(_mainCamera.transform);
    }

    private void OnDisable()
    {
        _inputReader.AimEvent -= Aim;
        _playerTransformAnchor.OnAnchorProvided -= SetupPlayerVirtualCamera;
        _onSwitchCamera.OnEventRaised -= SwitchToCamera;
        _cameraShakeEvent.OnEventRaised -= _impulseSource.GenerateImpulse;
    }

    private void Start()
    {
        virtualCams = GetComponentsInChildren<CinemachineVirtualCamera>();
        if (virtualCams.Length == 0)
            Debug.LogWarning("There is no virtual camera _ CameraManager.cs");

        if (_playerTransformAnchor.isSet)
            SetupPlayerVirtualCamera();
    }

    private void SetupPlayerVirtualCamera()
    {
        Transform target = _playerTransformAnchor.Value;

        foreach (var virtualCam in virtualCams)
        {
            virtualCam.Follow = target;
            virtualCam.LookAt = target;
            if (target != null)
                virtualCam.OnTargetObjectWarped(target, target.position - virtualCam.transform.position - Vector3.forward);
        }
    }

    public void SwitchToCamera()
    {
        if (_currentCamera.index < 0 || _currentCamera.index >= virtualCams.Length)
            return;

        for (int i = 0; i < virtualCams.Length; ++i)
        {
            virtualCams[i].Priority = (i == _currentCamera.index) ? 1 : 0;
        }
    }

    private void Aim(Vector2 normalDirection)
    {
        if (virtualCams[_currentCamera.index] != null)
        {
            CinemachineComposer composer = virtualCams[_currentCamera.index].GetCinemachineComponent<CinemachineComposer>();

            // 스무스 하게 하자
            composer.m_ScreenX = normalDirection.x == 0 ? 0.5f : 0.5f - 0.1f * normalDirection.x; 
            composer.m_ScreenY = normalDirection.y == 0 ? 0.65f : 0.65f + 0.3f * normalDirection.y;
        }
    }

    /* Execute in Animation Clip */
    public void ShakeCamera()
    {
        _cameraShakeEvent.RaiseEvent();
    }

    // private int _prevCameraIndex = -1; // 초기값: 불가능한 인덱스로 설정
    // private void OnValidate()
    // {
    //     if (_currentCamera != null && cameraIndex != _prevCameraIndex)
    //     {
    //         _currentCamera.index = cameraIndex;
    //         SwitchToCamera();
    //         _prevCameraIndex = cameraIndex;
    //     }
    // }
}
