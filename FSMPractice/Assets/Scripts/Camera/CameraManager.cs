using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputReader inputReader;
    public Camera mainCamera;

    [SerializeField] private TransformAnchor _playerTransformAnchor = default;
    [SerializeField] private CameraSO _currentCamera;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onSwitchCamera = default; 

    private CinemachineVirtualCamera[] virtualCams;

    private void OnEnable()
    {
        _playerTransformAnchor.OnAnchorProvided += SetupPlaerVirtualCamera;
        _onSwitchCamera.OnEventRaised += SwitchToCamera;
    }

    private void OnDisable()
    {
        _playerTransformAnchor.OnAnchorProvided -= SetupPlaerVirtualCamera;
    }

    private void Start()
    {
        virtualCams = GetComponentsInChildren<CinemachineVirtualCamera>();

        if (_playerTransformAnchor.isSet)
            SetupPlaerVirtualCamera();
    }

    private void SetupPlaerVirtualCamera()
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
