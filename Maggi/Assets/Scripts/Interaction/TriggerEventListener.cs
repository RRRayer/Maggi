using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TriggerEventListener : MonoBehaviour
{
    [SerializeField] private TimelineAsset _timeline = default;

    [Header("Broadcasting on")]
    [SerializeField] private TimelineAssetEventChannelSO _onStartTimeline = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_onStartTimeline != null && _timeline != null)
            {
                _onStartTimeline.RaiseEvent(_timeline);
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
