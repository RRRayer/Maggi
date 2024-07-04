using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPushingCondition", menuName = "State Machines/Conditions/is Pushing Condition")]
public class isPushingConditionSO : StateConditionSO<isPushingCondition> { }

public class isPushingCondition : Condition
{
	protected new isPushingConditionSO OriginSO => (isPushingConditionSO)base.OriginSO;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

	protected override bool Statement() => _interactionManager.pushInput;
}
