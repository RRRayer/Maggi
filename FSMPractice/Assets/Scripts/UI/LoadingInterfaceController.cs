using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingInterfaceController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingInterface = default;

    [Header("Listening on")]
    [SerializeField] private BoolEventChannellSO _toggleLoadingScreen = default;

    private void OnEnable()
    {
        _toggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
    }

    private void OnDisable()
    {
        _toggleLoadingScreen.OnEventRaised -= ToggleLoadingScreen;
    }

    private void ToggleLoadingScreen(bool state)
    {
        _loadingInterface.SetActive(state);
    }
}
