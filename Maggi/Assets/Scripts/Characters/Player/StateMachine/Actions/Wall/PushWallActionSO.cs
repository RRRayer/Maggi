using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

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

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
        _interactionManager = owner.GetComponent<InteractionManager>();
    }

    public override void OnStateEnter()
	{
		_player.movementVector = _transform.up * _originSO.pushForce + Vector3.up * _originSO.upForce;
		_interactionManager.currentInteractionType = InteractionType.None;
		_interactionManager.currentInteractiveObject = null;
	}

	public override void OnUpdate() 
	{ 
		
	}
}
