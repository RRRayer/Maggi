using UnityEngine;

[CreateAssetMenu(menuName = "Events/AudioCue Event Channel")]
public class AudioCueEventChannel : DescriptionBaseSO
{
   // public AudioCuePlay
}

public delegate AudioCueKey AudioCuePlayAction(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace);
public delegate bool AudioCueStopAction(AudioCueKey emitterKey);
public delegate bool AudioCueFinishAction(AudioCueKey emitterKey);
