using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class BossEventTrigger : MonoBehaviour
{
    [SerializeField] private TimelineAsset _timeline;

    [Header("Broadcasting on")]
    [SerializeField] private TransformEventChannelSO _moveToTargetEvent = default;
    [SerializeField] private TimelineAssetEventChannelSO _setTimelineAssetEvent = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _moveToTargetEvent.RaiseEvent(transform);
            _setTimelineAssetEvent.RaiseEvent(_timeline);

            gameObject.SetActive(false);
        }
    }
}
