using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushPointAction", menuName = "State Machines/Actions/Push Point Action")]
public class PushPointActionSO : StateActionSO<PushPointAction>
{
	public float pushForce;
}

public class PushPointAction : StateAction
{
	private PushPointActionSO _originSO => (PushPointActionSO)base.OriginSO;
	private InteractionManager _interactionManager;

	private Player _player;
	private Transform _transform;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
		_transform = stateMachine.GetComponent<Transform>();
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

	
    public override void OnStateEnter()
	{
		_player.movementVector = _transform.up;
		_player.movementVector = _player.movementVector.normalized * _originSO.pushForce;
	}

    public override void OnStateExit()
    {
        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;
    }

    public override void OnUpdate() { }
}
