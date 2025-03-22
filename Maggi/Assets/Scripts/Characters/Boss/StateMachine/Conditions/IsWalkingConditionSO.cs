using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using Maggi.Character.Boss;

[CreateAssetMenu(fileName = "IsWalkingCondition", menuName = "State Machines/Conditions/Boss/Is Walking Condition", order = 0)]
public class IsWalkingConditionSO : StateConditionSO<IsWalkingCondition> { }

public class IsWalkingCondition : Condition
{
	private Boss _boss;

	public override void Awake(StateMachine stateMachine)
	{
        _boss = stateMachine.GetComponent<Boss>();
    }
	
	protected override bool Statement()
	{
		return _boss.CurrentMode == Mode.Walk;
	}
}
