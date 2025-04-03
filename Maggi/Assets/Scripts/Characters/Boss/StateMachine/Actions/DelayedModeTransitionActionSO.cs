using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using Maggi.Character.Boss;

[CreateAssetMenu(fileName = "DelayedModeTransitionAction", menuName = "State Machines/Actions/Boss/Delayed Mode Transition Action", order = 0)]
public class DelayedModeTransitionActionSO : StateActionSO<DelayedModeTransitionAction>
{
    public float timerLength = 3.0f;
    public Mode mode;
}

public class DelayedModeTransitionAction : StateAction
{
	protected new DelayedModeTransitionActionSO _originSO => (DelayedModeTransitionActionSO)base.OriginSO;
	private float _startTime;
	private Boss _boss;

	public override void Awake(StateMachine stateMachine)
	{
		_boss = stateMachine.GetComponent<Boss>();
	}

    public override void OnStateEnter()
    {
        _startTime = Time.time;
    }

    public override void OnUpdate()
	{
        if (Time.time >= _startTime + _originSO.timerLength)
        {
            //Debug.Log("mode 설정");
            _boss.SetMode(_originSO.mode, "delayed mode transition action");
        }
    }
}
