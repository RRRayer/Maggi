using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPullingLightCondition", menuName = "State Machines/Conditions/is Pulling Light Condition")]
public class isPullingLightConditionSO : StateConditionSO<isPullingLightCondition> { }

public class isPullingLightCondition : Condition
{
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}
	
	protected override bool Statement()
	{
		if (_interactionManager.currentInteractionType == InteractionType.Light)
			return true;
		return false;
	}
}
