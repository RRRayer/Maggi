using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class InteractionEventListener : MonoBehaviour
{
    [SerializeField] private TimelineAsset _timeline = default;

    [Header("Broadcasting on")]
    [SerializeField] private TimelineAssetEventChannelSO _onStartTimeline = default;
    [SerializeField] private bool _isEnable;
    public bool IsEnable
    {
        set { _isEnable = value; }
        get { return _isEnable; }
    }

    [Header("If it is closed")]
    [SerializeField] private KeySO _requiredKey = default;
    public KeySO RequiredKey => _requiredKey;

    // Interact Action executes this event
    public void OnInteract()
    {
        if (_isEnable)
        {
            _onStartTimeline.RaiseEvent(_timeline);
            //_isEnable = false;
        }
    }
}