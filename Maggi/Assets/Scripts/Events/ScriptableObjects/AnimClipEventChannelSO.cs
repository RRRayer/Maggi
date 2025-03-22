using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/AnimClip Event Channel")]
public class AnimClipEventChannelSO : DescriptionBaseSO
{
    public UnityAction<AnimationClip> OnEventRaised;

    public void RaiseEvent(AnimationClip value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
