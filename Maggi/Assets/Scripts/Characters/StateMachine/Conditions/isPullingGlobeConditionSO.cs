using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPullingGlobeCondition", menuName = "State Machines/Conditions/is Pulling Globe Condition")]
public class isPullingGlobeConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new isPullingGlobeCondition();
}

public class isPullingGlobeCondition : Condition
{
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}
	
	protected override bool Statement()
	{
		if (_interactionManager.currentInteractionType == InteractionType.None)
			return true;
		return false;
	}
}
