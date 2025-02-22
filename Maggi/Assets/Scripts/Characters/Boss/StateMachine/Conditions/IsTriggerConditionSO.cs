using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsTriggerCondition", menuName = "State Machines/Conditions/Boss/Is Trigger Condition", order = 0)]
public class IsTriggerConditionSO : StateConditionSO<IsTriggerCondition> { }

public class IsTriggerCondition : Condition
{
	private Boss _boss;

	public override void Awake(StateMachine stateMachine)
	{
        _boss = stateMachine.GetComponent<Boss>();
    }
	
	protected override bool Statement()
	{
		return _boss.CurrentMode == Mode.Trigger;
	}
}
