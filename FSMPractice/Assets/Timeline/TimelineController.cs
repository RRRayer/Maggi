using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] private TimelineAssetEventChannelSO _onStartTimeline = default;

    private PlayableDirector _playableDirector;

    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        _onStartTimeline.OnEventRaised += StartTimeline;
    }

    private void OnDisable()
    {
        _onStartTimeline.OnEventRaised -= StartTimeline;
    }

    private void StartTimeline(TimelineAsset _timeline)
    {
        _playableDirector.Play(_timeline);
    }
}
