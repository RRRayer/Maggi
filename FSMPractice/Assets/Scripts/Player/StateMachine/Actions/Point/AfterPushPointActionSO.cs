using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AfterPushPointAction", menuName = "State Machines/Actions/After Push PointAction")]
public class AfterPushPointActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new AfterPushPointAction();
}

public class AfterPushPointAction : StateAction
{
	protected new AfterPushPointActionSO _originSO => (AfterPushPointActionSO)base.OriginSO;

	private Player _player;
    private Transform _transform;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
	}
	
	public override void OnStateEnter()
	{
		Debug.Log(_player.movementVector);
	}
	public override void OnUpdate()
	{
	}
}
