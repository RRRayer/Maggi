using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "IsCatchedCondition", menuName = "State Machines/Conditions/Is Catched Condition")]
public class IsCatchedConditionSO : StateConditionSO<IsCatchedCondition>
{
    [SerializeField]
    public string playableDirectorName; // PlayableDirector의 이름으로 찾을 수 있도록

    [SerializeField]
    public TimelineAsset[] timelineAsset;
}

public class IsCatchedCondition : Condition
{
    protected new IsCatchedConditionSO OriginSO => (IsCatchedConditionSO)base.OriginSO;

    private PlayableDirector _playableDirector;

    public override void Awake(StateMachine stateMachine)
    {
        // 지정된 이름을 가진 PlayableDirector를 씬에서 찾아서 할당
        _playableDirector = GameObject.Find(OriginSO.playableDirectorName)?.GetComponent<PlayableDirector>();

        if (_playableDirector == null)
        {
            Debug.LogError($"PlayableDirector '{OriginSO.playableDirectorName}'을(를) 찾을 수 없습니다!");
        }
    }

    protected override bool Statement()
    {
        return _playableDirector != null &&
               (_playableDirector.playableAsset == OriginSO.timelineAsset[0] || _playableDirector.playableAsset == OriginSO.timelineAsset[1]) &&
               _playableDirector.state == PlayState.Playing;
    }
}