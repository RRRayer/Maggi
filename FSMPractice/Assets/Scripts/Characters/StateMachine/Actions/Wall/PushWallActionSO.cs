using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushWallAction", menuName = "State Machines/Actions/Push Wall Action")]
public class PushWallActionSO : StateActionSO<PushWallAction> 
{
	public float pushForce;
	public float upForce;
}

public class PushWallAction : StateAction
{
	private PushWallActionSO _originSO => (PushWallActionSO)base.OriginSO;
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
		// 이거를 하려면 transform.up 이 아니고 xz 평면에 정사영한 방향으로 해야하는게 아닌가? 나중에  수정
		_player.movementVector = _transform.up * _originSO.pushForce + Vector3.up * _originSO.upForce;
		_interactionManager.currentInteractionType = InteractionType.None;
		_interactionManager.currentInteractiveObject = null;
	}

	public override void OnUpdate() { }
}
