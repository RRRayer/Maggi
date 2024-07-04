using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "TimeElapsedCondition", menuName = "State Machines/Conditions/Time Elapsed Condition")]
public class TimeElapsedConditionSO : StateConditionSO<TimeElapsedCondition>
{
	public float timerLength = 0.5f;
}

public class TimeElapsedCondition : Condition
{
	protected new TimeElapsedConditionSO OriginSO => (TimeElapsedConditionSO)base.OriginSO;
	private float _startTime;
	
	protected override bool Statement()
	{
		return Time.time >= _startTime + OriginSO.timerLength;
	}
	
	public override void OnStateEnter()
	{
		_startTime = Time.time;
	}
}
