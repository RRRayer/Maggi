using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using Maggi.Character.Boss;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "PlayTimeline", menuName = "State Machines/Actions/Boss/Play Timeline")]
public class PlayTimelineActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new PlayTimelineAction();
}

public class PlayTimelineAction : StateAction
{
	protected new PlayTimelineActionSO _originSO => (PlayTimelineActionSO)base.OriginSO;
    private Boss _boss;

	public override void Awake(StateMachine stateMachine)
	{
        
        _boss = stateMachine.GetComponent<Boss>();
	}

    public override void OnStateEnter()
    {
        _boss.timelineDirector.stopped += OnTimelineStopped;
        _boss.timelineDirector.Play();
        Debug.Log("타임라인 실행");
    }

    public override void OnStateExit()
    {
        _boss.timelineDirector.stopped -= OnTimelineStopped;
        _boss.timelineDirector.Stop();
        Debug.Log("타임라인 종료");
    }

    public override void OnUpdate()
	{
	}

    private void OnTimelineStopped(PlayableDirector director)
    {
        _boss.SetMode(Mode.Idle, "play timeline action");
    }
}
