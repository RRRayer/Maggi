using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

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
	
	public override void OnUpdate()
	{
		_characterController.Move(_playerScript.movementVector * Time.deltaTime);
		_playerScript.movementVector = _characterController.velocity;
	}
}
