using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsDetectCondition", menuName = "State Machines/Conditions/Boss/Is Detect Condition", order = 0)]
public class IsDetectConditionSO : StateConditionSO<IsDetectCondition> { }

public class IsDetectCondition : Condition
{
	private Boss _boss;

	public override void Awake(StateMachine stateMachine)
	{
        _boss = stateMachine.GetComponent<Boss>();
    }
	
	protected override bool Statement()
	{
		return _boss.CurrentMode == Mode.Detect;
	}
}
