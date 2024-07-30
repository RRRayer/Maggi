using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AlwaysTrueCondition", menuName = "State Machines/Conditions/Always True Condition")]
public class AlwaysTrueConditionSO : StateConditionSO<AlwaysTrueCondition> { }

public class AlwaysTrueCondition : Condition
{
	protected override bool Statement()
	{
		return true;
	}
}
