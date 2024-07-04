using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomEffect : MonoBehaviour
{
    public CinemachineVirtualCamera _virtualCamera;

    public float zoomedInFOV = 20f;
    public float zoomedOutFOV = 60f;
    public float zoomDuration = 1.0f;
    public float zoomThresholdDistance = 5.0f;

    public bool _isZooming = false;
    public float _targetFOV;
    public float _zoomStartTime;

    public Vector2 _center; // Zoom Collider center, X, Z 좌표에 대한 거리 계산
    public float _r; // Zoom Collider radious

    private void Awake()
    {
        if (_virtualCamera == null)
        {
            Debug.LogError("There is no virtual camera / CameraZoomEffect.cs");
            return;
        }
    }

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        _center = new Vector2(collider.bounds.center.x, collider.bounds.center.z);
        _r = collider.bounds.size.x / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartZoom(zoomedInFOV);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartZoom(zoomedOutFOV);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 playerPosition = new Vector2(other.transform.position.x, other.transform.position.z);
            float distance = Vector2.Distance(_center, playerPosition);

            if (distance < zoomThresholdDistance)
            {
                float t = distance / _r;
                float targetFOV = Mathf.Lerp(zoomedInFOV, zoomedOutFOV, t);
                StartZoom(targetFOV);
            }
                
            else
                StartZoom(zoomedOutFOV);
        }
    }

    private void Update()
    {
        if (_isZooming)
        {
            float t = (Time.time - _zoomStartTime) / zoomDuration;
            if (t >= 1.0f)
            {
                t = 1.0f;
                _isZooming = false;
            }

            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(_virtualCamera.m_Lens.FieldOfView, _targetFOV, t);
        }
    }

    private void StartZoom(float newFOV)
    {
        _targetFOV = newFOV;
        _zoomStartTime = Time.time;
        _isZooming = true;
    }
}
