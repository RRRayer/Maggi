using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveWithSieveWheelAction", menuName = "State Machines/Actions/Move With Sieve Wheel Action")]
public class MoveWithSieveWheelActionSO : StateActionSO<MoveWithSieveWheelAction>
{
    public float WheelSpeed;
}

public class MoveWithSieveWheelAction : StateAction
{
	private MoveWithSieveWheelActionSO _originSO => (MoveWithSieveWheelActionSO)base.OriginSO;

    private Player _player;
    private Transform _transform;
    private InteractiveObject _interactiveObject;
    private Rigidbody _interactionRigid;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
        _interactiveObject = interactiveObject;
    }

    public override void OnStateEnter()
    {
        _interactionRigid = _interactiveObject.GetComponent<Rigidbody>();
    }

    public override void OnUpdate()
    {
        _interactionRigid.velocity = _interactiveObject.transform.right * _player.movementInput.x * _originSO.WheelSpeed;
        _transform.position = _interactiveObject.transform.position - _interactiveObject.transform.up * 0.3f;
    }
}
