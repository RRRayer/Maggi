using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveWithSieveWheelAction", menuName = "State Machines/Actions/Move With Sieve Wheel Action")]
public class MoveWithSieveWheelActionSO : StateActionSO<MoveWithSieveWheelAction>
{
    [SerializeField]
    public float WheelSpeed;
}

public class MoveWithSieveWheelAction : StateAction
{


	protected new MoveWithSieveWheelActionSO _originSO => (MoveWithSieveWheelActionSO)base.OriginSO;

    private Player _player;
    private Transform _transform;
	private InteractionManager _interactionManager;
    private Rigidbody _interactionRigid;


    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnStateEnter()
    {
        _interactionRigid = _interactionManager.currentInteractiveObject.GetComponent<Rigidbody>();
    }

    public override void OnUpdate()
    {
        _interactionRigid.velocity = _interactionManager.currentInteractiveObject.transform.right * _player.movementInput.x * _originSO.WheelSpeed;
        _transform.position = _interactionManager.currentInteractiveObject.transform.position - _interactionManager.currentInteractiveObject.transform.up * 0.3f;
    }
}
