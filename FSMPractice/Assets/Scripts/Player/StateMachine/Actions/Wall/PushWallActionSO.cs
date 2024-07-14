using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushWallAction", menuName = "State Machines/Actions/Push Wall Action")]
public class PushWallActionSO : StateActionSO<PushWallAction> 
{
	public float pushForce;
}

public class PushWallAction : StateAction
{
	protected new PushWallActionSO _originSO => (PushWallActionSO)base.OriginSO;
	private InteractionManager _interactionManager;

	private Player _player;
	private Transform _transform;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
		_transform = stateMachine.GetComponent<Transform>();
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

	
    public override void OnStateExit()
	{
		_player.movementVector = _transform.up*_originSO.pushForce + Vector3.up*9.8f;
		_interactionManager.currentInteractionType = InteractionType.None;
		_interactionManager.currentInteractiveObject = null;
	}

	public override void OnUpdate()
	{
	}
	
}
