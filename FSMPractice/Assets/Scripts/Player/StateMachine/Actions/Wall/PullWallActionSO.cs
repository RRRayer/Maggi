using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;
using UnityEngine.UIElements;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "PullWallAction", menuName = "State Machines/Actions/Pull Wall Action")]

public class PullWallActionSO : StateActionSO<PullWallAction> 
{
    public LayerMask floorLayerMask;
}

public class PullWallAction : StateAction
{
	protected new PullWallActionSO _originSO => (PullWallActionSO)base.OriginSO;
	private Player _player;
    private Transform _transform;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
	}
	
	public override void OnUpdate()
	{
        _player.movementVector.y = 5.0f;
        
        Quaternion x = _interactionManager.currentInteractiveObject.transform.rotation;
        _transform.rotation = x;
	}
}
