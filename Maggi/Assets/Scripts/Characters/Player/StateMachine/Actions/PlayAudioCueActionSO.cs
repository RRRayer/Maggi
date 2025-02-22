using Maggi.StateMachine.ScriptableObjects;
using Maggi.StateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Actions/Play AudioCue")]
public class PlayAudioCueActionSO : StateActionSO<PlayAudioCueAction>
{
    public AudioCueSO audioCue = default;
    public AudioCueEventChannelSO audioCueEventChannel = default;
    public AudioConfigurationSO audioConfiguration = default;
}

public class PlayAudioCueAction : StateAction
{
    private Transform _ownerTransform;

    private PlayAudioCueActionSO _originSO => (PlayAudioCueActionSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine stateMachine)
    {
        _ownerTransform = stateMachine.transform;
    }

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _ownerTransform = owner.transform;
    }

    public override void OnUpdate() { }

    public override void OnStateEnter()
    {
        _originSO.audioCueEventChannel.RaisePlayEvent(_originSO.audioCue, _originSO.audioConfiguration, _ownerTransform.position);
    }
}