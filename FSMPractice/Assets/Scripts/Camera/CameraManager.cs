using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputReader inputReader;
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualCam;

    [SerializeField] private TransformAnchor _playerTransformAnchor = default;

    private void OnEnable()
    {
        _playerTransformAnchor.OnAnchorProvided += SetupPlaerVirtualCamera;
    }

    private void OnDisable()
    {
        _playerTransformAnchor.OnAnchorProvided -= SetupPlaerVirtualCamera;
    }

    private void Start()
    {
        if (_playerTransformAnchor.isSet)
            SetupPlaerVirtualCamera();
    }

    private void SetupPlaerVirtualCamera()
    {
        Transform target = _playerTransformAnchor.Value;

        virtualCam.Follow = target;
        // virtualCam.LookAt = target;
        virtualCam.OnTargetObjectWarped(target, target.position - virtualCam.transform.position - Vector3.forward);
    }
}
