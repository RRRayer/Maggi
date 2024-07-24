using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionTrigger : MonoBehaviour
{
    public int targetIndex = default;

    [SerializeField] private CameraSO _currentCamera;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _onSwitchCamera = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _currentCamera.index = targetIndex;
            _onSwitchCamera.RaiseEvent();
        }
    }
}
