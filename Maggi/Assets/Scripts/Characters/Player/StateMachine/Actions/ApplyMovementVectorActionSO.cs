using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyMovementVectorAction", menuName = "State Machines/Actions/Apply Movement Vector Action")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

public class ApplyMovementVectorAction : StateAction
{
	protected new ApplyMovementVectorActionSO OriginSO => (ApplyMovementVectorActionSO)base.OriginSO;

	private Player _playerScript;
	private CharacterController _characterController;

	public override void Awake(StateMachine stateMachine)
	{
        _playerScript = stateMachine.GetComponent<Player>();
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _playerScript = owner.GetComponent<Player>();
        _characterController = owner.GetComponent<CharacterController>();
    }

    public override void OnUpdate()
	{
		_characterController.Move(Time.deltaTime * _playerScript.movementVector * (_playerScript.runInput ? Player.RUN_SPEED_MULTIPLIER : 1));
	}
    public override void OnStateEnter()
    {
        _playerScript.movementVector = Vector3.zero;
    }
}
