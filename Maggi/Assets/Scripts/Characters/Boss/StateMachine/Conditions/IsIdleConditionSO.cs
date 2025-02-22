using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsIdleCondition", menuName = "State Machines/Conditions/Boss/Is Idle Condition", order = 0)]
public class IsIdleConditionSO : StateConditionSO<IsIdleCondition> { }

public class IsIdleCondition : Condition
{
	private Boss _boss;

	public override void Awake(StateMachine stateMachine)
	{
        _boss = stateMachine.GetComponent<Boss>();
    }
	
	protected override bool Statement()
	{
		return _boss.CurrentMode == Mode.Idle;
	}
}
