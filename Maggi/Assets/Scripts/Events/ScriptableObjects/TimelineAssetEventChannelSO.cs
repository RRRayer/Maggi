using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Events/Timeline Asset Event Channel")]
public class TimelineAssetEventChannelSO : DescriptionBaseSO
{
    public UnityAction<TimelineAsset> OnEventRaised;

    public void RaiseEvent(TimelineAsset value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
