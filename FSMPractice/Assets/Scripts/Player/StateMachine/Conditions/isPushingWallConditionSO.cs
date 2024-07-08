using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPushingWallCondition", menuName = "State Machines/Conditions/is Pushing Wall Condition")]
public class isPushingWallConditionSO : StateConditionSO<isPushingWallCondition> { }

public class isPushingWallCondition : Condition
{
	protected new isPushingWallConditionSO OriginSO => (isPushingWallConditionSO)base.OriginSO;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

	protected override bool Statement() => _interactionManager.pushInput;
}