using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class BossEventTrigger : MonoBehaviour
{
    [SerializeField] private TimelineAsset _timeline;

    [Header("Broadcasting on")]
    [SerializeField] private TransformEventChannelSO _moveToTargetEvent = default;
    [SerializeField] private TimelineAssetEventChannelSO _timelineEvent = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _moveToTargetEvent.RaiseEvent(transform);
            _timelineEvent.RaiseEvent(_timeline);
        }
    }
}
