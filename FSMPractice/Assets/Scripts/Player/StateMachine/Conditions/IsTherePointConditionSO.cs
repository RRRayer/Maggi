using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsTherePointCondition", menuName = "State Machines/Conditions/Is There Point Condition")]
public class IsTherePointConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsTherePointCondition();
}

public class IsTherePointCondition : Condition
{
	protected new IsTherePointConditionSO OriginSO => (IsTherePointConditionSO)base.OriginSO;

	public override void Awake(StateMachine stateMachine)
	{
	}
	
	protected override bool Statement()
	{
		return true;
	}
	
	public override void OnStateEnter()
	{
	}
	
	public override void OnStateExit()
	{
	}
}
