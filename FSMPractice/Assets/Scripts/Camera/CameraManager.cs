using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private TransformAnchor _playerTransformAnchor = default;
    [SerializeField] private TransformAnchor _cameraTransformAnchor = default;
    [SerializeField] private CameraSO _currentCamera;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onSwitchCamera = default; 

    private CinemachineVirtualCamera[] virtualCams;

    private void OnEnable()
    {
        _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;
        _onSwitchCamera.OnEventRaised += SwitchToCamera;
        _cameraTransformAnchor.Provide(_mainCamera.transform);

        SwitchToCamera();
    }

    private void OnDisable()
    {
        _playerTransformAnchor.OnAnchorProvided -= SetupPlayerVirtualCamera;
        _onSwitchCamera.OnEventRaised -= SwitchToCamera;
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
}
