using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

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

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
        _interactionManager = owner.GetComponent<InteractionManager>();
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

        int count = 0;
        while (_player.movementVector.y >= 0) { if (count++ > 10000) break; }
    }

    public override void OnUpdate() { }
}
